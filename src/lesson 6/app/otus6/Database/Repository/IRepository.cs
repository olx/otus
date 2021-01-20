using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using otus6.Models;

namespace otus6.Database
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(long id);
        Task<TEntity> Create(TEntity entity);
        Task<TEntity> Update(long id, TEntity entity);
        Task<TEntity> Delete(long id);
    }
}
