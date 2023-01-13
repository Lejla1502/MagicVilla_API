using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.CompilerServices;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/villaNumber")]
    [ApiController]
    public class VillaNumberController : ControllerBase
    {
        private readonly APIResponse _response;
        private readonly IVillaNumRepository _villaNumDB;
        private readonly IVillaRepository _villaDB;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumRepository villaNumDb, IVillaRepository villaDB, IMapper mapper)
        {
            this._response = new();
            _villaNumDB= villaNumDb;
            _villaDB = villaDB;
            _mapper= mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> villaNums = await _villaNumDB.GetAllAsync(includeProperties:"Villa");

                _response.Result = _mapper.Map<List<VillaNumberDto>>(villaNums);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch(Exception ex) { 
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                _response.IsSuccess = false;
            }

            return _response;
        }

        [HttpGet("{villaNo:int}", Name ="GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            try
            {
                if(villaNo == 0)
                {
                    _response.StatusCode= HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                VillaNumber vn = await _villaNumDB.GetAsync(v => v.VillaNo == villaNo, includeProperties:"Villa");

                if (vn == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaNumberDto>(vn);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() }; 
            }

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber(VillaNumberCreateDto createDto)
        {
            try
            {
                if(createDto.VillaNo == 0)
                {
                    _response.StatusCode= HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Villa number cannot be 0" };
                    return BadRequest(_response);
                }
                if (await _villaNumDB.GetAsync(x => x.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa Number already exists");
                    return BadRequest(ModelState);
                }

                if(await _villaDB.GetAsync(x=>x.Id == createDto.VillaID) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa with given ID doesn't exist");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                VillaNumber vn = _mapper.Map<VillaNumber>(createDto);
                await _villaNumDB.CreateAsync(vn);

                _response.Result = _mapper.Map<VillaNumberDto>(vn);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;

                return CreatedAtRoute("GetVillaNumber", new { villaNo = vn.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{villaNo:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int villaNo)
        {
            try
            {
                if(villaNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                VillaNumber vn = await _villaNumDB.GetAsync(v => v.VillaNo == villaNo);

                if(vn == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _villaNumDB.RemoveAsync(vn);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.ToString() };
            }

            return _response;
        }

        [HttpPut("{villaNo}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int villaNo, [FromBody] VillaNumberUpdateDto updateDto)
        {
            try
            {
                if(updateDto == null || villaNo!= updateDto.VillaNo)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (await _villaDB.GetAsync(x => x.Id == updateDto.VillaID) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa with given ID doesn't exist");
                    return BadRequest(ModelState);
                }

                VillaNumber vn = _mapper.Map<VillaNumber>(updateDto);

                await _villaNumDB.UpdateVillaNumAsync(vn);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.ErrorMessages = new List<string>() { e.ToString() };
                _response.IsSuccess = false;
            }

            return _response;
        }

       

    }
}
