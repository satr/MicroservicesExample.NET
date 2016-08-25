using Common;
using ProductsService.DataLayer;
using ServiceCommon;

namespace ProductsService
{
    public class ProductsServiceStartup: ServiceStartupBase
    {
        public ProductsServiceStartup()
        {
            ServiceLocator.BindSingletone<IProductsRepository>(new ProductsRepository());
        }
    }
}