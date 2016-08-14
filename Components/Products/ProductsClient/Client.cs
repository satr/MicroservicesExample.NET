using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MicroservicesExample.NET.ProductsService;

namespace ProductsServiceClient
{
    public class Client
    {
        private readonly string _hostUrl;
        private readonly HttpClient _httpClient = new HttpClient();

        public Client(string hostUrl)
        {
            _hostUrl = hostUrl;
        }

        public async Task<IList<Product>> GetAllProducts()
        {
            var products = new List<Product>();
            using (var response = await _httpClient.GetAsync(new Uri(new Uri(_hostUrl), "api/V1")))
            {
                var productSet = ProductSet.Parser.ParseFrom(await response.Content.ReadAsStreamAsync());
                if (productSet != null)
                    products = productSet.Items.ToList();
            }
            return products;
        }
    }
}
