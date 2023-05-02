using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //Expresion<Func<Villa>> respresents "Where" LINQ condition; bool is output
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, 
            int pageSize = 2, int pageNumber = 1);
        //tracked represents whether AsNoTracking is needed
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync(); //save changes to DB
    }
}
