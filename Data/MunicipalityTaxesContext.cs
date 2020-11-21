using Core.Domain.Municipalities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class MunicipalityTaxesContext : DbContext
    {
        public MunicipalityTaxesContext(DbContextOptions<MunicipalityTaxesContext> options)
            : base(options)
        {
        }

        public DbSet<Municipality> Municipalities { get; set; }

        public DbSet<MunicipalityTax> MunicipalityTaxes { get; set; }
    }
}
