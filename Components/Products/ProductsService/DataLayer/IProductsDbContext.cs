using System;
using System.Data.Entity;
using MicroservicesExample.NET.ProductsService;

namespace ProductsService.DataLayer
{
    public interface IProductsDbContext: IDisposable
    {
        IDbSet<Product> Products { get; set; }
        int SaveChanges();
    }
}