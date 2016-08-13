using Ninject;
using Ninject.Modules;

namespace ProductsService
{
    internal class ServiceLocator: NinjectModule
    {
        private static ServiceLocator _instance;
        private static readonly object SyncRoot = new object();

        public override void Load()
        {
            Bind<IProductRepository>().ToConstant(new ProductsRepository());
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
            return Instance.Kernel.Get<T>();
        }
    }
}