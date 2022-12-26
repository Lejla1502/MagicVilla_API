using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaNumberService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaNumberCreateDto createDto);
        Task<T> UpdateAsync<T>(VillaNumberUpdateDto updateDto);
        Task<T> DeleteAsync<T>(int id);
    }
}
