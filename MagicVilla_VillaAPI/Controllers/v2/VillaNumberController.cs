using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/villaNumber")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberV2Controller : ControllerBase
    {
        private readonly APIResponse _response;
        private readonly IVillaNumRepository _villaNumDB;
        private readonly IVillaRepository _villaDB;
        private readonly IMapper _mapper;

        public VillaNumberV2Controller(IVillaNumRepository villaNumDb, IVillaRepository villaDB, IMapper mapper)
        {
            _response = new();
            _villaNumDB = villaNumDb;
            _villaDB = villaDB;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }


    }

}
