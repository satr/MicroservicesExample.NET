using System;
using System.Collections.Generic;
using MicroservicesExample.NET.Products.Service;

namespace Products.Service.DataLayer
{
    public interface IProductsRepository: IDisposable
    {
        void Save(Product product);
        IList<Product> GetAll();
        void Set(IProductsDbContext dbContext);
    }
}