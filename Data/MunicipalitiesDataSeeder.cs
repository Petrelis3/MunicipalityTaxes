using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Domain.Municipalities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Data
{
    public class MunicipalitiesDataSeeder
    {
        private readonly MunicipalityTaxesContext context;
        private readonly IConfiguration configuration;

        public MunicipalitiesDataSeeder(
            MunicipalityTaxesContext context, 
            IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public void SeedData()
        {
            var pathToFile = configuration["DataSeeds:Municipalities"];
            if (!string.IsNullOrEmpty(pathToFile) && File.Exists(pathToFile))
            {
                var municipalities = new List<Municipality>();
                using (StreamReader r = new StreamReader(pathToFile))
                {
                    string json = r.ReadToEnd();
                    municipalities = JsonConvert.DeserializeObject<List<Municipality>>(json);
                }

                foreach (var municipality in municipalities)
                {
                    AddNewMunicipality(municipality);
                }

                context.SaveChanges();
            }
        }

        private void AddNewMunicipality(Municipality municipality)
        {
            var existingType = context.Municipalities.FirstOrDefault(p => p.Name == municipality.Name);
            if (existingType == null)
            {
                context.Municipalities.Add(municipality);
            }
        }
    }
}
