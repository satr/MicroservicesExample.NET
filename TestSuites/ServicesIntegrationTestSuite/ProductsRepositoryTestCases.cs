using System;
using System.Linq;
using MicroservicesExample.NET.ProductsService;
using NUnit.Framework;
using ProductsService.DataLayer;

namespace ServicesIntegrationTestSuite
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
