using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ProductsServiceClient;

namespace ServiceClientsIntergrationTestSuite
{
    [TestFixture]
    public class ProductsServiceClientTestCases
    {
        private static Process _process;
        private static Client _client;
        private const string DefaultHostUrl = "http://localhost:9003";
        private const string DefaultApiPingPath = "api/ping";
        private const string ApiPath = "api/v1";

        [OneTimeSetUp]
        public void ClassSetup()
        {
            
#if DEBUG
            var folderName = new FileInfo(GetType().Assembly.CodeBase.Substring(8)).DirectoryName;
            if(string.IsNullOrWhiteSpace(folderName))
                Assert.Fail("Service executable file is not found.");

            var servicePath = Path.Combine(folderName, @"..\..\..\..\Components\Products\ProductsService\bin\Debug\ProductsService.exe");
#else
            var servicePath = @"..\..\..\Components\Products\ProductsService\bin\Release\ProductsService.exe";
#endif
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
                Arguments = $"{DefaultHostUrl} {DefaultApiPingPath} -test2",
            };
            _process = new Process {StartInfo = processStartInfo};
            try
            {
                _process.Start();
                _client = new Client(DefaultHostUrl, ApiPath);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [OneTimeTearDown]
        public void ClassTearDown()
        {
            _process.StandardInput.WriteLine("");//"hit" Enter to exit the service.
            _process.WaitForExit(1000);
            if(!_process.HasExited)
                _process.Kill();
            _process.Close();
        }

        [Test]
        public void GetAllProductsTest()
        {
            var products = _client.GetAllProducts().Result;
            Assert.IsNotNull(products);
            Assert.IsTrue(products.Count > 0);
            var product = products.FirstOrDefault();
            Assert.IsNotNull(product);
            Assert.IsFalse(string.IsNullOrWhiteSpace(product.Id));
            Assert.IsFalse(string.IsNullOrWhiteSpace(product.Name));
        }

        [Test]
        public void AddNewProduct()
        {
            var product = Mother.CreateProduct();

            var operationResult = _client.Save(product).Result;

            Assert.IsTrue(operationResult.Success);
        }
    }
}
