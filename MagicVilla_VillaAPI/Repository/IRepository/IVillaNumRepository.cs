using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaNumRepository : IRepository<VillaNumber>
    {
        Task<VillaNumber> UpdateVillaNumAsync(VillaNumber entity);
    }
}
