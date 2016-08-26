using Common.Service;

namespace Products.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceManager.StartInConsole<ProductsServiceStartup>(args);
        }
    }
}
