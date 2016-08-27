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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf;
using log4net;
using NUnit.Framework;
using TestTools;
using TestTools.Service;

namespace Customers.Service.Integration.TestSuite
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
            StartService<CustomersServiceStartup>("http://localhost:9003");
            _httpClient = new HttpClient();
            var httpResponseMessage = _httpClient.PostAsync(GetRequestUri("api/management/settestrepo"), new StringContent(string.Empty)).Result;
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
        public void GetAllCustomers()
        {
            var customerSet = GetCustomerSet();

            Assert.IsNotNull(customerSet);
            Assert.AreEqual(3, customerSet.Items.Count);
        }

        private CustomerSet GetCustomerSet()
        {
            var serviceResponse = _httpClient.GetAsync(GetRequestUri("api/v1")).Result;
            var stream = serviceResponse.Content.ReadAsStreamAsync().Result;
            var customerSet = CustomerSet.Parser.ParseFrom(stream);
            return customerSet;
        }

        [Test]
        public void SaveNewProduct()
        {
            var origProductsCount = GetCustomerSet().Items.Count;
            var product = new Customer { Id = TestData.GetRandomGuidString(), Name = TestData.GetRandomString()};
            using (var stream = new MemoryStream())
            {
                product.WriteTo(stream);
                stream.Seek(0, SeekOrigin.Begin);

                var httpResponseMessage = SendPut(stream).Result;

                Assert.IsTrue((int) httpResponseMessage.StatusCode < 300);
            }
            var currProductsCount = GetCustomerSet().Items.Count;
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
