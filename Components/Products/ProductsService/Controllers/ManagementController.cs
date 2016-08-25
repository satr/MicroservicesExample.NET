using Common;
using ProductsService.DataLayer;
using ServiceCommon.Controllers;

namespace ProductsService.Controllers
{
    public class ManagementController: ManagementControllerBase
    {
        protected override void SetTestRepo()
        {
            ServiceLocator.Get<IProductsRepository>().Set(new TestProductsDbContext());
        }
    }
}
