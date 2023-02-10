using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    
    //to invoke endpoint we must define route
    //[Route("api/[controller]")] - if we leave it like this, Path udates automatically
    //if we change controller name
    [Route("api/v{version:apiVersion}/Villa")] //-but if we leave it like this, even if controller name changes,
    //path remains the same, that is api/Villa
    [ApiVersion("1.0")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly APIResponse _response;
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepository _villaDB;
        //private readonly ILogging _customILogger;
        private readonly IMapper _mapper;
    

        public VillaController(ILogger<VillaController> logger, IVillaRepository villaDb, IMapper mapper)
        {
            this._response = new();
            this._logger = logger;
            _villaDB = villaDb;
            _mapper = mapper;
           // _customILogger = customLogger;
        }
        //- it will not work without HttpGet
        [HttpGet]
       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<APIResponse>> GetVillas()
        {

            try
            {
                IEnumerable<Villa> villaList = await _villaDB.GetAllAsync();

                _logger.LogInformation("Getting all villas");
                //_customILogger.Log("Getting all villas", "");

                //automatically mapping 
                _response.Result = _mapper.Map<List<VillaDto>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }

            return _response;
        }

        //explicitely saying that id is integer, but we can omit that and just leave id
        [HttpGet("{id:int}", Name="GetVilla")]
        //to document all of the possible resonse types/codes
        ///[ProducesResponseType(typeof(int), 200)] //-OK
        //[ProducesResponseType(typeof(int), 404)] //-NOTFOUND
        //[ProducesResponseType(typeof(int), 400)] //-BAD REUEST

        //cleaner version
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                 var villa = await _villaDB.GetAsync(i => i.Id == id);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch(Exception e){
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _response;
            
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla(VillaCreateDto createDto)
        {
            try
            {
                if (await _villaDB.GetAsync(x => x.Name.ToLower() == createDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa already exists");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //if(!ModelState.IsValid)
                //    return BadRequest(ModelState);

                if (createDto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                Villa villa = _mapper.Map<Villa>(createDto);

                await _villaDB.CreateAsync(villa);

                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new {id=villa.Id}, _response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin")]
        //we use IActionResult because we don't want to define type nor return any data
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id) 
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaDB.GetAsync(x => x.Id == id);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _villaDB.RemoveAsync(villa);      //there is no RemoveAsync


                //usually used for delete; status coe 204
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id,[FromBody] VillaUpdateDto updateDto)
        {

            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //with ef core we don't need to retreive record in order to upda it
                //based on id, it can figure on its own which record needs to be updated

                Villa villa = _mapper.Map<Villa>(updateDto);
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

                await _villaDB.UpdateAsync(villa);


                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {

            try
            {
                if (patchDto == null || id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //here we need to retreive villa, because in patch document we dont get the whole object
                var villa = await _villaDB.GetAsync(x => x.Id == id, false);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

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
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                return NoContent();
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return _response;
        }
    }
}
