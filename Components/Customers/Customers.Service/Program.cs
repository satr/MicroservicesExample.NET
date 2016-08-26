using Common.Service;

namespace Customers.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceManager.StartInConsole<CustomersServiceStartup>(args);
        }
    }
}
