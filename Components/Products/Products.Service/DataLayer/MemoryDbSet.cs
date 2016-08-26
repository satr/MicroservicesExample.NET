using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Products.Service.DataLayer
{
    internal class MemoryDbSet<TEntity> : IDbSet<TEntity>
        where TEntity : class
    {
        public Type ElementType => typeof(TEntity);

        public Expression Expression => Expression.Empty();

        public ObservableCollection<TEntity> Local { get; } = new ObservableCollection<TEntity>();

        public IQueryProvider Provider => null;

        public TEntity Add(TEntity entity)
        {
            Local.Add(entity);
            return entity;
        }

        public TEntity Attach(TEntity entity)
        {
            //TODO
            return entity;
        }

        public TEntity Create()
        {
            return Activator.CreateInstance<TEntity>();
        }

        public TDerivedEntity Create<TDerivedEntity>() 
            where TDerivedEntity : class, TEntity
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public TEntity Find(params object[] keyValues)
        {
            return Local.FirstOrDefault();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Local.GetEnumerator();
        }

        public TEntity Remove(TEntity entity)
        {
            if (Local.Contains(entity))
                Local.Remove(entity);
            return entity;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Local.GetEnumerator();
        }
    }
}