using System;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace Core.Domain.Municipalities
{
    [Table(nameof(MunicipalityTax))]
    public class MunicipalityTax : BaseEntity
    {
        public TaxScheduleType TaxScheduleType { get; protected set; }

        public DateTime ValidFrom { get; protected set; }

        public DateTime ValidTo { get; protected set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Tax { get; protected set; }

        [ForeignKey(nameof(Municipality))]
        public Guid MunicipalityId { get; protected set; }

        public Municipality Municipality { get; protected set; }

        public void Update(decimal tax)
        {
            Tax = tax;
        }

        public static class Factory
        {
            public static MunicipalityTax Create(
                TaxScheduleType taxScheduleType,
                DateTime validFrom,
                DateTime validTo,
                decimal tax,
                Guid municipalityId)
            {
                var obj = new MunicipalityTax
                {
                    Id = Guid.NewGuid(),
                    TaxScheduleType = taxScheduleType,
                    ValidFrom = validFrom,
                    ValidTo = validTo,
                    Tax = tax,
                    MunicipalityId = municipalityId
                };

                var validator = new Validator();
                var validationResult = validator.Validate(obj);

                if (validationResult.Errors.Count > 0)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                return obj;
            }
        }

        private class Validator : AbstractValidator<MunicipalityTax>
        {
            public Validator()
            {
                RuleFor(q => q.Id).NotEmpty();
                RuleFor(q => q.TaxScheduleType).NotEmpty();
                RuleFor(q => q.ValidFrom).NotEmpty();
                RuleFor(q => q.ValidTo).NotEmpty().GreaterThan(q => q.ValidFrom);
                RuleFor(q => q.Tax).NotEmpty().GreaterThanOrEqualTo(0);
                RuleFor(q => q.MunicipalityId).NotEmpty();
            }
        }
    }
}
