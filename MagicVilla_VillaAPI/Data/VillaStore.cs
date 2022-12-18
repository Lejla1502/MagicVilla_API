using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> listVillas = new List<VillaDto>() 
                                    { new VillaDto { Id=1, Name="aaaa", Occupancy=2, Sqft=100 },
                                    new VillaDto {Id=2, Name="bbbbb", Occupancy=3, Sqft=120} };
    }
}
