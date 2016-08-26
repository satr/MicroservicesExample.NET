using Common;
using Common.Service.Controllers;
using Products.Service.DataLayer;

namespace Products.Service.Controllers
{
    public class ManagementController: ManagementControllerBase
    {
        protected override void SetTestRepo()
        {
            ServiceLocator.Get<IProductsRepository>().Set(new TestProductsDbContext());
        }
    }
}
