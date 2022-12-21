using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    
    //to invoke endpoint we must define route
    //[Route("api/[controller]")] - if we leave it like this, Path udates automatically
    //if we change controller name
    [Route("api/Villa")] //-but if we leave it like this, even if controller name changes,
    //path remains the same, that is api/Villa
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepository _villaDB;
        //private readonly ILogging _customILogger;
        private readonly IMapper _mapper;
    

        public VillaController(ILogger<VillaController> logger, IVillaRepository villaDb, IMapper mapper)
        {
            this._logger = logger;
            _villaDB = villaDb;
            _mapper = mapper;
           // _customILogger = customLogger;
        }
        //- it will not work without HttpGet
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            
            IEnumerable<Villa> villaList = await _villaDB.GetAllAsync();

            _logger.LogInformation("Getting all villas");
            //_customILogger.Log("Getting all villas", "");

            //automatically mapping 
            return Ok(_mapper.Map<List<VillaDto>>(villaList));
        }

        //explicitely saying that id is integer, but we can omit that and just leave id
        [HttpGet("{id:int}", Name="GetVilla")]
        //to document all of the possible resonse types/codes
        ///[ProducesResponseType(typeof(int), 200)] //-OK
        //[ProducesResponseType(typeof(int), 404)] //-NOTFOUND
        //[ProducesResponseType(typeof(int), 400)] //-BAD REUEST

        //cleaner version
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            { 
                _logger.LogInformation("Get villa error with id"+id);
               // _customILogger.Log("Get villa error with id "+id, "error");

                return BadRequest(); 
            }

            var villa = await _villaDB.GetAsync(i => i.Id == id);

            if(villa == null)   
                return NotFound();
            
            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaCreateDto>> CreateVilla(VillaCreateDto createDto)
        {
            if (await _villaDB.GetAsync(x => x.Name.ToLower() == createDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
            }

            //if(!ModelState.IsValid)
            //    return BadRequest(ModelState);

            if(createDto == null) 
                return BadRequest();

            

            Villa model = _mapper.Map<Villa>(createDto);

            await _villaDB.CreateAsync(model);

            return CreatedAtRoute("GetVilla", new {id=model.Id}, model);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        //we use IActionResult because we don't want to define type nor return any data
        public async Task<IActionResult> DeleteVilla(int id) 
        {
            if(id==0)
                return BadRequest();

            var villa= await _villaDB.GetAsync(x=>x.Id==id);

            if(villa == null)
                return NotFound();

            await _villaDB.RemoveAsync(villa);      //there is no RemoveAsync
           

            //usually used for delete; status coe 204
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id,[FromBody] VillaUpdateDto updateDto)
        {
            
            if(updateDto == null || id!= updateDto.Id)
                return BadRequest();

            //with ef core we don't need to retreive record in order to upda it
            //based on id, it can figure on its own which record needs to be updated

            Villa model = _mapper.Map<Villa>(updateDto);
            //Villa model = new Villa
            //{
            //    Id = updateDto.Id,
            //    Name = updateDto.Name,
            //    DateCreated = DateTime.Now,
            //    Amenity = updateDto.Amenity,
            //    Details = updateDto.Details,
            //    ImageUrl = updateDto.ImageUrl,
            //    Occupancy = updateDto.Occupancy,
            //    Rate = updateDto.Rate,
            //    Sqft = updateDto.Sqft
            //};

           await _villaDB.UpdateAsync(model);

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {

            if (patchDto == null || id ==0)
                return BadRequest();

            //here we need to retreive villa, because in patch document we dont get the whole object
            var villa = await _villaDB.GetAsync(x => x.Id == id, false);

            if(villa == null)
                return NotFound();

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
            //    new ()
            //{
            //    Id = villa.Id,
            //    Name = villa.Name,
            //    Amenity = villa.Amenity,
            //    Details = villa.Details,
            //    ImageUrl = villa.ImageUrl,
            //    Occupancy = villa.Occupancy,
            //    Rate = villa.Rate,
            //    Sqft = villa.Sqft
            //};


            patchDto.ApplyTo(villaDto, ModelState);

            Villa model = _mapper.Map<Villa>(villaDto);

            await _villaDB.UpdateAsync(model);

            if (!ModelState.IsValid) 
                return BadRequest();

            return NoContent();
        }
    }
}
