using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Google.Protobuf;
using MicroservicesExample.NET.ProductsService;

namespace ProductsService.Controllers
{
    public class V1Controller : ApiController
    {
        // GET api/values 
        public HttpResponseMessage Get()
        {
            var productSet = new ProductSet();
            for (int i = 0; i < 3; i++)
            {
                productSet.Items.Add(new Product() { Id = Guid.NewGuid().ToString(), Name = "Product" + i });
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var stream = new MemoryStream();
            productSet.WriteTo(stream);
            stream.Seek(0, SeekOrigin.Begin);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentLength = stream.Length;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return response;
        }
    }
}
