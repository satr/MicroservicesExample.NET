using System;
using System.Net;
using System.Net.Http;
using CustomersService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceCommon;

namespace ServicesIntegrationTestSuite
{
    [TestClass]
    public class CustomersServiceTestCases : ServiceTestCasesBase
    {
        private static HttpClient _httpClient;

        [ClassInitialize]
        public static void SetupClass(TestContext testContext)
        {
            StartService<CustomersServiceStartup>();
            _httpClient = new HttpClient();
        }

        [ClassCleanup]
        public static void TearDownClass()
        {
            StopService();
            _httpClient?.Dispose();
        }

        [TestMethod]
        public void TestPing()
        {
            var serviceResponse = _httpClient.GetAsync(new UriBuilder(HostUrl) { Path = "api/ping" }.Uri).Result;
            Assert.AreEqual(HttpStatusCode.OK, serviceResponse.StatusCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(serviceResponse.Content.ReadAsStringAsync().Result));
        }
    }
}
