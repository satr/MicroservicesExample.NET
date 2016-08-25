using System.Collections.Generic;
using System.Linq;
using MicroservicesExample.NET.ProductsService;

namespace ProductsService.DataLayer
{
    public class ProductsRepository: RepositoryBase<IProductsDbContext>, IProductsRepository
    {
        protected override IProductsDbContext CreateDbContext()
        {
            return new ProductsDbContext("Products");
        }

        public void Save(Product product)
        {
            DbContext.Products.Add(product);
            DbContext.SaveChanges();
        }

        public IList<Product> GetAll()
        {
            return DbContext.Products.ToList();
        }
    }
}