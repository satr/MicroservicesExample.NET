using System;
using System.Data.Entity;
using MicroservicesExample.NET.Products.Service;

namespace Products.Service.DataLayer
{
    public interface IProductsDbContext: IDisposable
    {
        IDbSet<Product> Products { get; set; }
        int SaveChanges();
    }
}