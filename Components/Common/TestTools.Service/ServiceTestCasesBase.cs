using System;
using System.Threading;
using Common.Service;
using log4net;

namespace TestTools.Service
{
    public abstract class ServiceTestCasesBase
    {
        // ReSharper disable once InconsistentNaming
        private static ILog Log = LogManager.GetLogger(typeof(ServiceTestCasesBase));
        private CancellationTokenSource _cancellationTokenSource;
        protected string HostUrl = "http://localhost:9000";//default url

        protected void StartService<T>(string httpLocalhost) 
            where T : ServiceStartupBase, new()
        {
            Log = LogManager.GetLogger(GetType());
            HostUrl = httpLocalhost;
            _cancellationTokenSource = ServiceManager.Start<T>(HostUrl);
            Log.Debug($"Service started in test cases on URL: {HostUrl}.");
        }

        protected void StopService()
        {
            Log.Debug("Stop the service in test cases.");
            if (_cancellationTokenSource == null)
                return;
            var autoResetEvent = new AutoResetEvent(false);
            _cancellationTokenSource.Token.Register(() => autoResetEvent.Set());
            _cancellationTokenSource.Cancel();
            autoResetEvent.WaitOne();
            Log.Debug("Service stopped in test cases.");
        }

        protected Uri GetRequestUri(string path)
        {
            return new UriBuilder(HostUrl) { Path = path }.Uri;
        }
    }
}