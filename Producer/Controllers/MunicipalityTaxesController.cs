using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Domain.Municipalities;
using Data;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Producer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MunicipalityTaxesController : ControllerBase
    {
        private readonly IMunicipalityTaxCalculationHelper taxesCalculationHelper;
        private readonly MunicipalityTaxesContext municipalityTaxesContext;

        public MunicipalityTaxesController(
            IMunicipalityTaxCalculationHelper taxesCalculationHelper,
            MunicipalityTaxesContext municipalityTaxesContext)
        {
            this.taxesCalculationHelper = taxesCalculationHelper;
            this.municipalityTaxesContext = municipalityTaxesContext;
        }

        [HttpGet("{name}/{date}")]
        public async Task<ActionResult<decimal>> Get(string name, DateTime date)
        {
            var municipality = await GetMunicipality(name);

            if (municipality == null)
            {
                return NotFound($"Municipality with name {name} was not found");
            }

            var tax = taxesCalculationHelper.GetValid(municipality.MunicipalityTaxes, date);

            if (tax == null)
            {
                return NotFound($"Municipality with name {name} has no valid taxes");
            }

            return tax.Tax;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Models.MunicipalityTax model)
        {
            var municipality = await GetMunicipality(model.MunicipalityName);

            if (municipality == null)
            {
                return NotFound($"Municipality with name {model.MunicipalityName} was not found");
            }

            try
            {
                if (municipality.MunicipalityTaxes?.Any(q => 
                    q.TaxScheduleType == model.TaxScheduleType
                    && q.ValidFrom == model.ValidFrom
                    && q.ValidTo == model.ValidTo) == true)
                {
                    throw new ValidationException($"Municipality with name {model.MunicipalityName} already has tax with schedule type {model.TaxScheduleType} with same valid period");
                }

                var tax = MunicipalityTax.Factory.Create(model.TaxScheduleType, model.ValidFrom, model.ValidTo, model.Tax, municipality.Id);

                municipalityTaxesContext.MunicipalityTaxes.Add(tax);
                await municipalityTaxesContext.SaveChangesAsync();
            } 
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(Models.MunicipalityTax model)
        {
            var municipality = await GetMunicipality(model.MunicipalityName);

            if (municipality == null)
            {
                return NotFound($"Municipality with name {model.MunicipalityName} was not found");
            }

            try
            {
                var tax = municipality.MunicipalityTaxes?.Where(q => q.TaxScheduleType == model.TaxScheduleType && q.ValidFrom == model.ValidFrom && q.ValidTo == model.ValidTo)?.FirstOrDefault();

                if (tax == null)
                {
                    throw new ValidationException($"Municipality with name {model.MunicipalityName} has no tax with schedule type {model.TaxScheduleType} and period {model.ValidFrom} - {model.ValidTo}");
                }

                tax.Update(model.Tax);                

                await municipalityTaxesContext.SaveChangesAsync();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Ok();
        }

        private async Task<Municipality> GetMunicipality(string name)
        {
            return await municipalityTaxesContext.Municipalities.Include(q => q.MunicipalityTaxes)
                .Where(q => q.Name == name).FirstOrDefaultAsync();
        }
    }
}
