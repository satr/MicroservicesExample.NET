using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace ServiceCommon
{
    public static class ServiceManager
    {
        const string DefaultHostUrl = "http://localhost:9000";
        const string DefaultRequestPath = "api/ping";

        public static void StartInConsole<T>(string[] args)
            where T : ServiceStartupBase, new()
        {
            StartInConsole<T>(args.Length > 0? args[0]: DefaultHostUrl, 
                                    args.Length > 1? args[1]: DefaultRequestPath);
        }

        public static void StartInConsole<T>(string hostUrl = DefaultHostUrl, string requestPath = DefaultRequestPath) 
            where T : ServiceStartupBase, new()
        {
            try
            {
                using (var cancellationTokenSource = Start<T>(hostUrl))
                {
                    var assemblyName = Assembly.GetEntryAssembly().GetName();
                    Console.Out.WriteLine("Service: {0}", assemblyName.Name);
                    Console.Out.WriteLine("Version: {0}", assemblyName.Version);
                    try
                    {
                        var response = new HttpClient().GetAsync(new Uri(new Uri(hostUrl), requestPath), cancellationTokenSource.Token).Result;
                        Console.Out.WriteLine("Status code: {0} (\"{1}\").", (int) response.StatusCode, response.StatusCode);
                        Console.Out.WriteLine("Respond text: {0}", response.Content.ReadAsStringAsync().Result);
                        Console.Out.WriteLine("Hit Enter to exit.");
                    }
                    catch (Exception e)
                    {
                        var builder = new StringBuilder();
                        builder.AppendLine("Error:");
                        while (e != null)
                        {
                            builder.AppendLine(e.Message);
                            e = e.InnerException;
                        }
                        Console.Out.WriteLine("Error: {0}", builder);
                    }
                    Console.ReadKey();
                    cancellationTokenSource.Cancel();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        public static CancellationTokenSource Start<T>(string hostUrl)
            where T: ServiceStartupBase, new()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Task.Factory.StartNew(() =>
                                    {
                                        using (WebApp.Start<T>(url: hostUrl))
                                        {
                                            token.WaitHandle.WaitOne();
                                        }
                                    }, token);
            return tokenSource;
        }
    }
}
