using System.Net;
using System.Net.Http;
using log4net;
using NUnit.Framework;
using ProductsService;

namespace ServicesIntegrationTestSuite
{
    [TestFixture]
    public class CustomersServiceTestCases : ServiceTestCasesBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomersServiceTestCases));
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void Setup()
        {
            Log.Debug("---------------- Started test cases session ----------------");
            StartService<ProductsServiceStartup>("http://localhost:9001");
            _httpClient = new HttpClient();
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
    }
}
