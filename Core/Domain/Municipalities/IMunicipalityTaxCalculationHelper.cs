using System;
using System.Collections.Generic;

namespace Core.Domain.Municipalities
{
    public interface IMunicipalityTaxCalculationHelper
    {
        MunicipalityTax GetValid(IEnumerable<MunicipalityTax> municipalityTaxes, DateTime date);
    }
}
