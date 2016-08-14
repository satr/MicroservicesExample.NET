using ServiceCommon;

namespace ProductsService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceManager.StartInConsole<ProductsServiceStartup>(args);
        }
    }
}
