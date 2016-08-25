using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Google.Protobuf;
using log4net;
using MicroservicesExample.NET.ProductsService;
using ProductsService.DataLayer;
using ProductsService.Validators;
using ServiceCommon;

namespace ProductsService.Controllers
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
            var productsRepository = ServiceLocator.Get<IProductsRepository>();
            foreach (var product in productsRepository.GetAll())
            {
                productSet.Items.Add(product);
            }
            _log.Debug($"Found {productSet.Items.Count} products.");
            return CreateHttpResponseMessageFor(productSet);
        }

        private HttpResponseMessage CreateHttpResponseMessageFor(IMessage protobufMessage)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var stream = new MemoryStream();
            protobufMessage.WriteTo(stream);
            stream.Seek(0, SeekOrigin.Begin);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentLength = stream.Length;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return response;
        }


        [HttpPut]
        public void PutAsync()
        {
            var ticket = 0;
            this.PerformOperation(_log, ticket, "Save product", requestContentStream =>
            {
                var product = Product.Parser.ParseFrom(requestContentStream);
                var validationResult = _validator.Validate(product);
                if (validationResult.Success)
                {
                    _log.Debug($"The product: id=\"{product.Id}\"; name=\"{product.Name}\"");
                    ServiceLocator.Get<IProductsRepository>().Save(product);
                }
                return validationResult;
            });
        }
    }
}
