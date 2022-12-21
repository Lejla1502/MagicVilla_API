using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly VillaDbContext _dbContext;

        public VillaRepository(VillaDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
       
        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.DateUpdated = DateTime.Now;
            _dbContext.Villas.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        } 
    }
}
