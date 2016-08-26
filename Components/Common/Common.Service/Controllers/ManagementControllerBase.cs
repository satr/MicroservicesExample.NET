using System.Web.Http;

namespace Common.Service.Controllers
{
    public abstract class ManagementControllerBase: ApiController
    {
        [HttpPost]
        [ActionName("settestrepo")]
        public void SetTestRepoAsync()
        {
            SetTestRepo();
        }

        protected abstract void SetTestRepo();
    }
}
