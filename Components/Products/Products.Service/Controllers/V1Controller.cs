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
using System.Net.Http;
using System.Web.Http;
using Common;
using Common.Service;
using log4net;
using Products.Service.DataLayer;
using Products.Service.Validators;

namespace Products.Service.Controllers
{
    public class V1Controller : ApiController
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(V1Controller));
        private readonly IValidator<Product> _validator;

        public V1Controller()
        {
            _validator = new ProductValidator();
        }

        [HttpGet]
        public HttpResponseMessage GetAsync()
        {
            _log.Debug("Request all products.");
            var productSet = new ProductSet();
            var repository = ServiceLocator.Get<IProductsRepository>();
            foreach (var product in repository.GetAll())
                productSet.Items.Add(product);

            _log.Debug($"Found {productSet.Items.Count} products.");
            return this.CreateHttpResponseMessageFor(productSet);
        }


        [HttpPut]
        public void PutAsync()
        {
            var ticket = 0;
            this.PerformOperation(_log, ticket, "Save product.", requestContentStream =>
            {
                var product = Product.Parser.ParseFrom(requestContentStream);
                var validationResult = _validator.Validate(product);
                if (validationResult.Success)
                {
                    _log.Debug($"The product: id=\"{product.Id}\"; name=\"{product.Name}\"; price=\"{product.Price}\"");
                    ServiceLocator.Get<IProductsRepository>().Save(product);
                }
                return validationResult;
            });
        }
    }
}
