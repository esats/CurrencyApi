using Microsoft.EntityFrameworkCore;
using ProteinCase.Dal.Abstract;
using ProteinCase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProteinCase.Dal.Concreate
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly CurrencyContext _dbContext;

        public Repository(CurrencyContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<int> Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            int result = await _dbContext.SaveChangesAsync();
            return result;
        }

        public T AddReturnEntity(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return entity;
        }

        public async Task<bool> Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            int result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
