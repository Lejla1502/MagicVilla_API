using MagicVilla_VillaAPI.DataStore;
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
        private readonly ILogging _customILogger;

        public VillaController(ILogger<VillaController> logger, ILogging customLogger)
        {
            this._logger = logger;
            _customILogger = customLogger;
        }
        //- it will not work without HttpGet
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Getting all villas");
            _customILogger.Log("Getting all villas", "");
            return Ok(VillaStore.listVillas);
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
                _customILogger.Log("Get villa error with id "+id, "error");

                return BadRequest(); 
            }

            var villa = VillaStore.listVillas.FirstOrDefault(x => x.Id == id);

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
            if (VillaStore.listVillas.FirstOrDefault(x => x.Name.ToLower() == villa.Name.ToLower()) != null)
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

            villa.Id= VillaStore.listVillas.OrderByDescending(x => x.Id).FirstOrDefault().Id+1;

            VillaStore.listVillas.Add(villa);

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

            var villa= VillaStore.listVillas.FirstOrDefault(x=>x.Id==id);

            if(villa == null)
                return NotFound();

            VillaStore.listVillas.Remove(villa);

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

            var v = VillaStore.listVillas.FirstOrDefault(x=>x.Id == id);

            v.Name = villa.Name;
            v.Occupancy = villa.Occupancy;
            v.Sqft = villa.Sqft;

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

            var v = VillaStore.listVillas.FirstOrDefault(x => x.Id == id);

            if(v == null)
                return NotFound();

            patchDto.ApplyTo(v, ModelState);

            if(!ModelState.IsValid) 
                return BadRequest();

            return NoContent();
        }
    }
}
