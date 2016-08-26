using System;
using System.Linq;
using MicroservicesExample.NET.Products.Service;
using NUnit.Framework;
using Products.Service.DataLayer;

namespace Products.Service.Integration.TestSuite
{
    [TestFixture]
    public class ProductsRepositoryTestCases
    {
        private IProductsRepository _productsRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            _productsRepository = new ProductsRepository();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _productsRepository.Dispose();
        }


        [Test, Ignore("Testing the repository connected to the real DB")]
        public void CreateNewProduct()
        {
            var id = Guid.NewGuid().ToString();
            var origProduct = new Product { Id = id, Name = "Test product" };
            _productsRepository.Save(origProduct);
            var product = _productsRepository.GetAll().FirstOrDefault(e => e.Id == id);
            Assert.IsNotNull(product);
            Assert.AreEqual(origProduct.Name, product.Name);
            Assert.AreEqual(origProduct.Price, product.Price);
        }
    }
}
