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
        public static void StartInConsole<T>(string hostUrl, string tryingRequestPath)
            where T : ServiceStartupBase, new()
        {
            using (var cancellationTokenSource = ServiceManager.Start<T>(hostUrl))
            {
                var assemblyName = Assembly.GetEntryAssembly().GetName();
                Console.Out.WriteLine("Service: {0}", assemblyName.Name);
                Console.Out.WriteLine("Version: {0}", assemblyName.Version);
                Console.Out.WriteLine(ServiceManager.TryRequestService(hostUrl, tryingRequestPath));
                Console.Out.WriteLine("Hit Enter to exit.");
                Console.ReadLine();
                cancellationTokenSource.Cancel();
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

        public static ServiceResponse TryRequestService(string hostUrl, string sequestPath)
        {
            try
            {
                var response = new HttpClient().GetAsync(new Uri(new Uri(hostUrl), sequestPath)).Result;
                return new ServiceResponse(response.StatusCode, response.Content.ReadAsStringAsync().Result);
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
                return new ServiceResponse(HttpStatusCode.ExpectationFailed, builder.ToString());
            }
        }

    }
}
