using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using otus6.Database;
using otus6.Models;
using Microsoft.EntityFrameworkCore;

namespace otus6.Database
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> Get(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            var result = _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<TEntity> Update(long id, TEntity entity)
        {
            var _entity = await _dbSet.FindAsync(id);
            if (_entity == null)
                return null;
            var result = _context.Entry(_entity);
            result.CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<TEntity> Delete(long id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return null;

            var result = _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
