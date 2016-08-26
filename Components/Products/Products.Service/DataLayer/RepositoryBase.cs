using System;

namespace Products.Service.DataLayer
{
    public abstract class RepositoryBase<T>
        where T: class, IDisposable
    {
        private T _dbContext;
        private bool _disposed;
        private readonly object _syncRoot = new object();

        protected T DbContext
        {
            get {
                lock (_syncRoot)
                {
                    return _dbContext?? (_dbContext = CreateDbContext());
                }
            }
        }

        protected abstract T CreateDbContext();

        public void Set(T dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _dbContext?.Dispose();
            _disposed = false;
        }
    }
}