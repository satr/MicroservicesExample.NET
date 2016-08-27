#region Copyright notice and license
// Copyright 2016 github.com/satr.  All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//     * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following disclaimer
// in the documentation and/or other materials provided with the
// distribution.
//     * Neither the name of github.com/satr nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Products.ServiceClient.Intergration.TestSuite
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
            
            var folderName = new FileInfo(GetType().Assembly.CodeBase.Substring(8)).DirectoryName;
            if(string.IsNullOrWhiteSpace(folderName))
                Assert.Fail("TestSuite dll file is not found.");

#if DEBUG
            var servicePath = Path.Combine(folderName, @"..\..\..\Products.Service\bin\Debug\Products.Service.exe");
#else
            var servicePath = Path.Combine(folderName, @"..\..\..\Products.Service\bin\Release\Products.Service.exe");
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
            if (_process == null)
                return;
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
