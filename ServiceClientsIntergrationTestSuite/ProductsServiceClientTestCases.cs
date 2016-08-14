using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductsServiceClient;

namespace ServiceClientsIntergrationTestSuite
{
    [TestClass]
    public class ProductsServiceClientTestCases
    {
        private static Process _process;
        const string DefaultHostUrl = "http://localhost:9000";
        const string DefaultRequestPath = "api/ping";

        [ClassInitialize]
        public static void ClassSetup(TestContext testContext)
        {
            var servicePath = @"..\..\..\Components\Products\ProductsService\bin\Debug\ProductsService.exe";
            if (!File.Exists(servicePath))
            {
                Assert.Fail("Service executable file is not found.");
            }
            var processStartInfo = new ProcessStartInfo(servicePath)
            {
                
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                ErrorDialog = false,
                UseShellExecute = false,
                Arguments = $"{DefaultHostUrl} {DefaultRequestPath}",
            };
            _process = new Process {StartInfo = processStartInfo};
            try
            {
                _process.Start();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [ClassCleanup]
        public static void ClassTearDown()
        {
            _process.StandardInput.WriteLine("");//"hit" Enter to exit the service.
            _process.WaitForExit(1000);
            if(!_process.HasExited)
                _process.Kill();
            _process.Close();
        }

        [TestMethod]
        public void GetAllProductsTest()
        {
            var products = new Client(DefaultHostUrl).GetAllProducts().Result;
            Assert.IsNotNull(products);
            Assert.IsTrue(products.Count > 0);
            var product = products.FirstOrDefault();
            Assert.IsNotNull(product);
            Assert.IsFalse(string.IsNullOrWhiteSpace(product.Id));
            Assert.IsFalse(string.IsNullOrWhiteSpace(product.Name));
        }
    }
}
