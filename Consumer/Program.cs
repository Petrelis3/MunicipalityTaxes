using System;
using System.Threading.Tasks;
using Core.Domain.Municipalities;

namespace Consumer
{
    class Program
    {
        public static readonly ConsumerEndPoint consumerEndPoint = new ConsumerEndPoint("https://localhost:44300/api/MunicipalityTaxes");

        static async Task Main()
        {
            Console.WriteLine("MunicipalityTax application demo");

            // Get municipality tax
            Console.WriteLine(await consumerEndPoint.Get("Copenhagen", DateTime.Today));

            // Insert municipality tax
            var newTax = new Models.MunicipalityTax
            {
                MunicipalityName = "Copenhagen",
                TaxScheduleType = TaxScheduleType.Daily,
                Tax = 0.7M,
                ValidFrom = DateTime.Today,
                ValidTo = DateTime.Today.AddDays(1)
            };

            Console.WriteLine(await consumerEndPoint.Post(newTax));

            // Edit municipality tax
            var taxToEdit = new Models.MunicipalityTax
            {
                MunicipalityName = "Copenhagen",
                TaxScheduleType = TaxScheduleType.Daily,
                Tax = 0.3M,
                ValidFrom = DateTime.Today,
                ValidTo = DateTime.Today.AddDays(1)
            };

            Console.WriteLine(await consumerEndPoint.Put(taxToEdit));

            Console.ReadKey();
        }
    }
}
