using MicroservicesExample.NET.ProductsService;
using TestTools;

namespace ServiceClientsIntergrationTestSuite
{
    public static class Mother
    {
        public static Product CreateProduct()
        {
            return new Product {Id = TestData.GetRandomGuidString(), Name = TestData.GetRandomString()};
        }
    }
}