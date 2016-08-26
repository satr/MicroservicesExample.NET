using Common;
using Common.Service;
using Products.Service.DataLayer;

namespace Products.Service
{
    public class ProductsServiceStartup: ServiceStartupBase
    {
        public ProductsServiceStartup()
        {
            ServiceLocator.BindSingletone<IProductsRepository>(new ProductsRepository());
        }
    }
}