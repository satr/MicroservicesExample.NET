using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.ServiceClient;
using Google.Protobuf;

namespace Products.ServiceClient
{
    public class Client: ClientBase
    {
        public Client(string hostUrl, string apiPath) : base(hostUrl, apiPath)
        {
        }

        public async Task<IList<Product>> GetAllProducts()
        {
            using (var response = await GetAsync(ApiPath))
            {
                var productSet = ProductSet.Parser.ParseFrom(await response.Content.ReadAsStreamAsync());
                return productSet.Items.ToList();
            }
        }

        public async Task<OperationResult> Save(IMessage product)
        {
            return await PutAsync(product, ApiPath);
        }
    }
}
