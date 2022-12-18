using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
        private readonly VillaDbContext _dbContext;
        //private readonly ILogging _customILogger;

        public VillaController(ILogger<VillaController> logger, VillaDbContext dbContext)
        {
            this._logger = logger;
            _dbContext = dbContext;
           // _customILogger = customLogger;
        }
        //- it will not work without HttpGet
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Getting all villas");
            //_customILogger.Log("Getting all villas", "");
            return Ok(_dbContext.Villas.ToList());
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
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            { 
                _logger.LogInformation("Get villa error with id"+id);
               // _customILogger.Log("Get villa error with id "+id, "error");

                return BadRequest(); 
            }

            var villa = _dbContext.Villas.FirstOrDefault(x => x.Id == id);

            if(villa == null)   
                return NotFound();
            
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CreateVilla(VillaDto villa)
        {
            if (_dbContext.Villas.FirstOrDefault(x => x.Name.ToLower() == villa.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
            }

            //if(!ModelState.IsValid)
            //    return BadRequest(ModelState);

            if(villa==null) 
                return BadRequest();

            if (villa.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError); //because it doesnt exist 
            //among default error messages

            Villa model = new Villa
            {
                Name=villa.Name,
                DateCreated = DateTime.Now,
                Amenity = villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };

            _dbContext.Villas.Add(model);
            _dbContext.SaveChanges();

            return CreatedAtRoute("GetVilla", new {id=villa.Id}, villa);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        //we use IActionResult because we don't want to define type nor return any data
        public IActionResult DeleteVilla(int id) 
        {
            if(id==0)
                return BadRequest();

            var villa= _dbContext.Villas.FirstOrDefault(x=>x.Id==id);

            if(villa == null)
                return NotFound();

            _dbContext.Villas.Remove(villa);
            _dbContext.SaveChanges();

            //usually used for delete; status coe 204
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id,[FromBody] VillaDto villa)
        {
            
            if(villa == null || id!=villa.Id)
                return BadRequest();

            //var v = _dbContext.Villas.FirstOrDefault(x=>x.Id == id);

            //v.Name = villa.Name;
            //v.Occupancy = villa.Occupancy;
            //v.Sqft = villa.Sqft;

            //with ef core we don't need to retreive record in order to upda it
            //based on id, it can figure on its own which record needs to be updated

            Villa model = new Villa
            {
                Id = villa.Id,
                Name = villa.Name,
                DateCreated = DateTime.Now,
                Amenity = villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };

            _dbContext.Villas.Update(model);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {

            if (patchDto == null || id ==0)
                return BadRequest();

            //here we need to retreive villa, because in patch document we dont get the whole object
            var villa = _dbContext.Villas.FirstOrDefault(x => x.Id == id);

            if(villa == null)
                return NotFound();

            VillaDto villaDto = new ()
            {
                Id = villa.Id,
                Name = villa.Name,
                Amenity = villa.Amenity,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };


            patchDto.ApplyTo(villaDto, ModelState);

            Villa model = new Villa()
            {
                Id = villaDto.Id,
                Name = villaDto.Name,
                Amenity = villaDto.Amenity,
                Details = villaDto.Details,
                ImageUrl = villaDto.ImageUrl,
                Occupancy = villaDto.Occupancy,
                Rate = villaDto.Rate,
                Sqft = villaDto.Sqft
            };

            _dbContext.Update(model);
            _dbContext.SaveChanges();

            if (!ModelState.IsValid) 
                return BadRequest();

            return NoContent();
        }
    }
}
