using System.Web.Http;

namespace ServiceCommon.Controllers
{
    public abstract class PingControllerBase : ApiController
    {
        // GET api/values 
        public string Get()
        {
            return "OK";
        }
    }
}
