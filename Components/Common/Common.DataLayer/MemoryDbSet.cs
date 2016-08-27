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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Common.DataLayer
{
    public class MemoryDbSet<TEntity> : IDbSet<TEntity>
        where TEntity : class
    {
        public Type ElementType => typeof(TEntity);

        public Expression Expression => Expression.Empty();

        public ObservableCollection<TEntity> Local { get; } = new ObservableCollection<TEntity>();

        public IQueryProvider Provider => null;//TODO

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