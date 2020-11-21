using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace Core.Domain.Municipalities
{
    [Table(nameof(Municipality))]
    public class Municipality : BaseEntity
    {
        [StringLength(300)]
        public string Name { get; set; }

        public ICollection<MunicipalityTax> MunicipalityTaxes { get; protected set; }

        public static class Factory
        {
            public static Municipality Create(string name)
            {
                var obj = new Municipality
                {
                    Id = Guid.NewGuid(),
                    Name = name
                };

                var validator = new Validator();
                var validationResult = validator.Validate(obj);

                if (validationResult.Errors.Count > 0)
                {
                    throw new FluentValidation.ValidationException(validationResult.Errors);
                }

                return obj;
            }
        }

        private class Validator : AbstractValidator<Municipality>
        {
            public Validator()
            {
                RuleFor(q => q.Id).NotEmpty();
                RuleFor(q => q.Name).NotEmpty().MaximumLength(300);
            }
        }
    }
}
