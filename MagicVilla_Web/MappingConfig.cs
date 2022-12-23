using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile //Profile is inside the automapper
    {
        public MappingConfig() 
        {
            //ReverseMap means mapping goes both ways
            //Villa to VillaDto and VillaDto to Villa
            CreateMap<VillaDto, VillaCreateDto>().ReverseMap(); 
            CreateMap<VillaDto, VillaUpdateDto>().ReverseMap();

            //Villa Number
            CreateMap<VillaNumberDto, VillaNumberCreateDto>().ReverseMap();
            CreateMap<VillaNumberDto, VillaNumberUpdateDto>().ReverseMap();

        }
    }
}
