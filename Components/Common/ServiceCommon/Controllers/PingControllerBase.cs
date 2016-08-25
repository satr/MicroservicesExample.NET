using System.Web.Http;
using log4net;

namespace ServiceCommon.Controllers
{
    public abstract class PingControllerBase : ApiController
    {
        private static ILog _log;
        private static readonly object SyncRoot = new object();
        // GET api/values 
        public string Get()
        {
            lock (SyncRoot)
            {
                if(_log == null)
                    _log = LogManager.GetLogger(GetType());
            }
            _log.Debug("Ping.");
            return "OK";
        }
    }
}
