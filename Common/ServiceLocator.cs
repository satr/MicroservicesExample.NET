using System.Linq;
using Ninject;
using Ninject.Modules;

namespace Common
{
    public class ServiceLocator: NinjectModule
    {
        private static ServiceLocator _instance;
        private static readonly object SyncRoot = new object();
        private readonly IKernel _kernel;

        public ServiceLocator()
        {
            _kernel = new StandardKernel(this);
        }

        public static ServiceLocator Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    return _instance ?? (_instance = new ServiceLocator());
                }
            }
        }

        public static T Get<T>()
        {
            return Instance._kernel.Get<T>();
        }

        public override void Load()
        {
        }

        public static void BindSingletone<T>(T implementation)
        {
            if(Instance.Kernel.GetBindings(typeof(T)).Any())
                Instance.Rebind<T>().ToConstant(implementation);
            else
                Instance.Bind<T>().ToConstant(implementation);
        }
    }
}