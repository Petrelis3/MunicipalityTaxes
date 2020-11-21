using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Municipalities;

namespace Business.Municipalities
{
    public class MunicipalityTaxCalculationHelper : IMunicipalityTaxCalculationHelper
    {
        public MunicipalityTax GetValid(IEnumerable<MunicipalityTax> municipalityTaxes, DateTime date)
        {
            return municipalityTaxes?.Where(q => q.ValidFrom <= date && q.ValidTo >= date)?.OrderByDescending(q => q.TaxScheduleType)?.FirstOrDefault();
        }
    }
}
