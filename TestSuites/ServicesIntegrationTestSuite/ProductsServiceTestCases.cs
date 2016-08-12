using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductsService;
using ServiceCommon;

namespace ServicesIntegrationTestSuite
{
    [TestClass]
    public class ProductsServiceTestCases : ServiceTestCasesBase
    {
        [ClassInitialize]
        public static void SetupClass(TestContext testContext)
        {
            StartService<ProductsServiceStartup>();
        }

        [ClassCleanup]
        public static void TearDownClass()
        {
            StopService();
        }

        [TestMethod]
        public void TestPing()
        {
            var serviceResponse = ServiceManager.TryRequestService(HostUrl, "api/ping");
            Assert.AreEqual(HttpStatusCode.OK, serviceResponse.StatusCode);
            Assert.IsFalse(string.IsNullOrWhiteSpace(serviceResponse.RespondResult));
        }
    }
}
