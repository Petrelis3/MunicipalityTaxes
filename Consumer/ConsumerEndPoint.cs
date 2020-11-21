using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Consumer.Models;
using Newtonsoft.Json;

namespace Consumer
{
    public class ConsumerEndPoint
    {
        private readonly string apiUrl;

        public ConsumerEndPoint(string apiUrl)
        {
            this.apiUrl = apiUrl;
        }

        public async Task<string> Get(string name, DateTime date)
        {
            var result = string.Empty;

            try
            {
                var clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                using var client = new HttpClient(clientHandler);
                var response = await client.GetAsync($"{apiUrl}/{name}/{date}");
                result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<string> Post(MunicipalityTax model)
        {
            var result = string.Empty;

            try
            {
                var clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                using var client = new HttpClient(clientHandler);
                var response = await client.PostAsync(
                    apiUrl,
                    new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
               
                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<string> Put(MunicipalityTax model)
        {
            var result = string.Empty;

            try
            {
                var clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                using var client = new HttpClient(clientHandler);
                var response = await client.PutAsync(
                    apiUrl,
                    new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

                result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }
    }
}
