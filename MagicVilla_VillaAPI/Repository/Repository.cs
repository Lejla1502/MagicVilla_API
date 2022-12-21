using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly VillaDbContext _dbContext;
        internal DbSet<T> dbSet;

        public Repository(VillaDbContext dbContext)
        {
            _dbContext = dbContext;
            this.dbSet = _dbContext.Set<T>();   
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            //when we work with IQueryable, it doesn't get executed right away, so we can additionally 
            //build on this query
            IQueryable<T> query = dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);


            ///at this point the query will be executed 
            //it is DEFERRED EXECUTION
            //ToList() causes immediate execution
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            //when we work with IQueryable, it doesn't get executed right away, so we can additionally 
            //build on this query
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);


            ///at this point the query will be executed 
            //it is DEFERRED EXECUTION
            //ToList() causes immediate execution
            return await query.ToListAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

    }
}
