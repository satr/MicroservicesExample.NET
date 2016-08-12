using System.Net;
using CustomersService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceCommon;

namespace ServicesIntegrationTestSuite
{
    [TestClass]
    public class CustomersServiceTestCases : ServiceTestCasesBase
    {
        [ClassInitialize]
        public static void SetupClass(TestContext testContext)
        {
            StartService<CustomersServiceStartup>();
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
