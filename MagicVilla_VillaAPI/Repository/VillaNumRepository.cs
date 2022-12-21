using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumRepository : Repository<VillaNumber>, IVillaNumRepository
    {
        private readonly VillaDbContext _dbContext;

        public VillaNumRepository(VillaDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<VillaNumber> UpdateVillaNumAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
