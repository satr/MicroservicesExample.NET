using System;
using System.Data.Entity;
using MicroservicesExample.NET.Products.Service;

namespace Products.Service.DataLayer
{
    public class TestProductsDbContext : IProductsDbContext
    {
        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        public TestProductsDbContext()
        {
            Populate();
        }

        private void Populate()
        {
            Products = new MemoryDbSet<Product>
            {
                CreateTestProduct(),
                CreateTestProduct(),
                CreateTestProduct()
            };
        }

        private Product CreateTestProduct()
        {
            return new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = $"test-{Guid.NewGuid()}",
                Price = _random.Next(3, 7)
            };
        }

        public void Dispose()
        {
            Populate();
        }

        public IDbSet<Product> Products { get; set; }

        public int SaveChanges()
        {
            return 0;
        }
    }
}