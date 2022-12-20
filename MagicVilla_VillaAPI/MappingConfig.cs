using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile //Profile is inside the automapper
    {
        public MappingConfig() 
        {
            //ReverseMap means mapping goes both ways
            //Villa to VillaDto and VillaDto to Villa
            CreateMap<Villa, VillaDto>().ReverseMap();

            CreateMap<Villa, VillaCreateDto>().ReverseMap(); 

            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
        }
    }
}
