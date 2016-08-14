using ServiceCommon;

namespace CustomersService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceManager.StartInConsole<CustomersServiceStartup>(args);
        }
    }
}
