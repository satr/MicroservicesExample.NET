using ServiceCommon;

namespace CustomersService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceManager.StartInConsole<CustomersServiceStartup>("http://localhost:9000", "api/ping");
        }
    }
}
