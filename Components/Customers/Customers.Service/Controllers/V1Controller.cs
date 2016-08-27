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
using System.Net.Http;
using System.Web.Http;
using Common;
using Common.Service;
using Customers.Service.DataLayer;
using Customers.Service.Validators;
using log4net;

namespace Customers.Service.Controllers
{
    public class V1Controller : ApiController
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(V1Controller));
        private readonly IValidator<Customer> _validator;

        public V1Controller()
        {
            _validator = new CustomerValidator();
        }

        [HttpGet]
        public HttpResponseMessage GetAsync()
        {
            _log.Debug("Request all products.");
            var customerSet = new CustomerSet();
            var repository = ServiceLocator.Get<ICustomersRepository>();
            foreach (var customer in repository.GetAll())
                customerSet.Items.Add(customer);

            _log.Debug($"Found {customerSet.Items.Count} customers.");
            return this.CreateHttpResponseMessageFor(customerSet);
        }


        [HttpPut]
        public void PutAsync()
        {
            var ticket = 0;
            this.PerformOperation(_log, ticket, "Save customer.", requestContentStream =>
            {
                var customer = Customer.Parser.ParseFrom(requestContentStream);
                var validationResult = _validator.Validate(customer);
                if (validationResult.Success)
                {
                    _log.Debug($"The customer: id=\"{customer.Id}\"; name=\"{customer.Name}\"; address=\"{customer.Address}\"");
                    ServiceLocator.Get<ICustomersRepository>().Save(customer);
                }
                return validationResult;
            });
        }

    }
}
