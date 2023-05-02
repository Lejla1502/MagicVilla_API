using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, 
            string? includeProperties = null)
        {
            //when we work with IQueryable, it doesn't get executed right away, so we can additionally 
            //build on this query
            IQueryable<T> query = dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);


            if(!includeProperties.IsNullOrEmpty())
            {
                foreach( var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }

            ///at this point the query will be executed 
            //it is DEFERRED EXECUTION
            //ToList() causes immediate execution
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, 
            string? includeProperties = null, int pageSize = 3, int pageNumber = 1)
        {
            //when we work with IQueryable, it doesn't get executed right away, so we can additionally 
            //build on this query
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            //paging
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }

                //first run: skip(3*(1-1)).take(3) - returns first three villas
                //second run: skip(3*(2-1)).take(3) - skips first three, and takes following three
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }

            if (!includeProperties.IsNullOrEmpty())
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }

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
