using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductsService;
using ServiceCommon;

namespace ServicesIntegrationTestSuite
{
    [TestClass]
    public class ProductsServiceTestCases : ServiceTestCasesBase
    {
        private static HttpClient _httpClient;

        [ClassInitialize]
        public static void SetupClass(TestContext testContext)
        {
            StartService<ProductsServiceStartup>();
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
            var serviceResponse = _httpClient.GetAsync(GetRequestUri("api/ping")).Result;
            Assert.AreEqual(HttpStatusCode.OK, serviceResponse.StatusCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(serviceResponse.Content.ReadAsStringAsync().Result));
        }

        private static Uri GetRequestUri(string path)
        {
            return new UriBuilder(HostUrl) { Path = path }.Uri;
        }

        [TestMethod]
        public void GetAllProducts()
        {
            var serviceResponse = _httpClient.GetAsync(GetRequestUri("api/v1")).Result;
            var stream = serviceResponse.Content.ReadAsStreamAsync().Result;
            var readByte = stream.ReadByte();
            Assert.IsFalse(readByte == -1);
        }

    }
}
