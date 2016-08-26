using TestTools;

namespace Products.ServiceClient.Intergration.TestSuite
{
    public static class Mother
    {
        public static Product CreateProduct()
        {
            return new Product {Id = TestData.GetRandomGuidString(), Name = TestData.GetRandomString()};
        }
    }
}