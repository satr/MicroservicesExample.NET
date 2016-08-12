using System.Threading;
using ServiceCommon;

namespace ServicesIntegrationTestSuite
{
    public abstract class ServiceTestCasesBase
    {
        private static CancellationTokenSource _cancellationTokenSource;
        protected static string HostUrl { get; private set; }

        protected static void StartService<T>()
            where T: ServiceStartupBase, new()
        {
            HostUrl = "http://localhost:9000";
            _cancellationTokenSource = ServiceManager.Start<T>(HostUrl);
        }

        protected static void StopService()
        {
            if (_cancellationTokenSource == null)
                return;
            var autoResetEvent = new AutoResetEvent(false);
            _cancellationTokenSource.Token.Register(() => autoResetEvent.Set());
            _cancellationTokenSource.Cancel();
            autoResetEvent.WaitOne();
        }
    }
}