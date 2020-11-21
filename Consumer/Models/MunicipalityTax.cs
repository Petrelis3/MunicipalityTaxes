using System;
using Core.Domain.Municipalities;

namespace Consumer.Models
{
    public class MunicipalityTax
    {
        public string MunicipalityName { get; set; }

        public TaxScheduleType TaxScheduleType { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public decimal Tax { get; set; }
    }
}
