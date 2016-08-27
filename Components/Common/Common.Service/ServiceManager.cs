#region Copyright notice and license
// Copyright 2016 github.com/satr.  All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//     * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following disclaimer
// in the documentation and/or other materials provided with the
// distribution.
//     * Neither the name of github.com/satr nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.Owin.Hosting;

namespace Common.Service
{
    public static class ServiceManager
    {
        private const string DefaultHostUrl = "http://localhost:9000";
        private const string DefaultApiPingPath = "api/ping";

        public static void StartInConsole<T>(string[] args)
            where T : ServiceStartupBase, new()
        {
            var hostUrl = GetParameter(args, "hosturl:");
            var path = GetParameter(args, "path:");
            StartInConsole<T>(hostUrl?? DefaultHostUrl, path?? DefaultApiPingPath, args.ToList());
        }

        private static string GetParameter(IEnumerable<string> args, string parameterName)
        {
            var parameter = args.FirstOrDefault(param => param.StartsWith(parameterName));
            if (string.IsNullOrWhiteSpace(parameter))
                return null;
            return parameter.Length <= parameterName.Length ? string.Empty : parameter.Substring(parameterName.Length);
        }

        public static void StartInConsole<T>(string hostUrl = DefaultHostUrl, string requestPath = DefaultApiPingPath)
            where T : ServiceStartupBase, new()
        {
            StartInConsole<T>(hostUrl, requestPath, new string[0]);
        }

        private static void StartInConsole<T>(string hostUrl, string requestPath, ICollection<string> parameters) 
            where T : ServiceStartupBase, new()
        {
            try
            {
                var log = LogManager.GetLogger(typeof(T));
                log.Info($"Start a self-hosting service on URL: {hostUrl}.");
                using (var cancellationTokenSource = Start<T>(hostUrl))
                {
                    var assemblyName = Assembly.GetEntryAssembly().GetName();
                    Console.Out.WriteLine($"Service: {assemblyName.Name}");
                    Console.Out.WriteLine($"Version: {assemblyName.Version}");
                    try
                    {
                        var httpClient = new HttpClient();
                        var requestUri = GetRequestUri(hostUrl, requestPath);
                        WriteLine(log, $"Ping the service: {requestUri}.");
                        var response = httpClient.GetAsync(requestUri, cancellationTokenSource.Token).Result;
                        WriteLine(log, $"Status code: {(int) response.StatusCode} (\"{response.StatusCode}\").");
                        WriteLine(log, $"Respond text: {response.Content.ReadAsStringAsync().Result}");
                        if (parameters.Contains("-test"))
                        {
                            var responseMessage = httpClient.PostAsync(GetRequestUri(hostUrl, "api/management/settestrepo"), new StringContent(string.Empty), cancellationTokenSource.Token).Result;
                            WriteLine(log,(int) responseMessage.StatusCode < 300
                                            ? "Set test repository"
                                            : $"Failed to set test repository {responseMessage.Content.ReadAsStringAsync().Result}");
                        }
                        Console.Out.WriteLine("Hit Enter to exit.");
                    }
                    catch (Exception e)
                    {
                        log.Error(e);
                        var builder = new StringBuilder();
                        builder.AppendLine("Error:");
                        while (e != null)
                        {
                            builder.AppendLine(e.Message);
                            e = e.InnerException;
                        }
                        Console.Out.WriteLine($"Error: {builder}");
                    }
                    Console.In.ReadLine();
                    log.Info($"Stop a self-hosting service on URL: {hostUrl}.");
                    cancellationTokenSource.Cancel();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.In.ReadLine();
            }
        }

        private static void WriteLine(ILog log, string message)
        {
            Console.Out.WriteLine(message);
            log.Debug(message);
        }

        private static Uri GetRequestUri(string hostUrl, string requestPath)
        {
            return new UriBuilder(hostUrl) { Path = requestPath }.Uri;
        }

        public static CancellationTokenSource Start<T>(string hostUrl)
            where T: ServiceStartupBase, new()
        {
            var log = LogManager.GetLogger(typeof(T));
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            Task.Factory.StartNew(() =>
                                    {
                                        using (WebApp.Start<T>(url: hostUrl))
                                        {
                                            log.Debug("Service started.");
                                            token.WaitHandle.WaitOne();
                                            log.Debug("Service stopped.");
                                        }
                                    }, token);
            return tokenSource;
        }
    }
}
