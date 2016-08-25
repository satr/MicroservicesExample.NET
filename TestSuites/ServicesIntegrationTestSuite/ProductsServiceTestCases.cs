using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf;
using log4net;
using MicroservicesExample.NET.ProductsService;
using NUnit.Framework;
using ProductsService;
using TestTools;

namespace ServicesIntegrationTestSuite
{
    [TestFixture]
    public class ProductsServiceTestCases : ServiceTestCasesBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProductsServiceTestCases));
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void Setup()
        {
            Log.Debug("---------------- Started test cases session ----------------");
            StartService<ProductsServiceStartup>("http://localhost:9002");
            _httpClient = new HttpClient();
            var httpResponseMessage = _httpClient.PostAsync(GetRequestUri("api/management/testrepo"), new StringContent(string.Empty)).Result;
            var httpStatusCode = httpResponseMessage.StatusCode;
        }

        [OneTimeTearDown]
        public void TearDownClass()
        {
            StopService();
            _httpClient?.Dispose();
            Log.Debug("---------------- Stopped test cases session ----------------");
        }

        [Test]
        public void TestPing()
        {
            var serviceResponse = _httpClient.GetAsync(GetRequestUri("api/ping")).Result;
            Assert.AreEqual(HttpStatusCode.OK, serviceResponse.StatusCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(serviceResponse.Content.ReadAsStringAsync().Result));
        }

        [Test]
        public void GetAllProducts()
        {
            var productSet = GetProductSet();

            Assert.IsNotNull(productSet);
            Assert.AreEqual(3, productSet.Items.Count);
        }

        private ProductSet GetProductSet()
        {
            var serviceResponse = _httpClient.GetAsync(GetRequestUri("api/v1")).Result;
            var stream = serviceResponse.Content.ReadAsStreamAsync().Result;
            var productSet = ProductSet.Parser.ParseFrom(stream);
            return productSet;
        }

        [Test]
        public void SaveNewProduct()
        {
            var origProductsCount = GetProductSet().Items.Count;
            var product = new Product {Id = TestData.GetRandomGuidString(), Name = TestData.GetRandomString()};
            using (var stream = new MemoryStream())
            {
                product.WriteTo(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var httpResponseMessage = SendPut(stream).Result;

                Assert.IsTrue((int) httpResponseMessage.StatusCode < 300);
            }
            var currProductsCount = GetProductSet().Items.Count;
            Assert.AreEqual(origProductsCount + 1, currProductsCount);
        }

        [Test]
        public void SetTestRepo()
        {
            var httpResponseMessage = _httpClient.PostAsync(GetRequestUri("api/management/settestrepo"), new StringContent(string.Empty)).Result;
            Assert.IsTrue((int)httpResponseMessage.StatusCode < 300);
        }

        private async Task<HttpResponseMessage> SendPut(Stream stream)
        {
            return await _httpClient.PutAsync(GetRequestUri("api/v1"), new StreamContent(stream))
                .ConfigureAwait(continueOnCapturedContext: false);
        }

    }
}
