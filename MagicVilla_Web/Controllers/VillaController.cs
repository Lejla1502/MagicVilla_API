using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDto> list = new();

            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess) {
                //we get all datafrom response in response.Result, we deserialize it, convert it to 
                //List of VillaDto type and assign it to our list
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<IActionResult> CreateVilla()
        {
            //List<VillaDto> list = new();

            //var response = await _villaService.GetAllAsync<APIResponse>();
            //if (response != null && response.IsSuccess)
            //{
            //    //we get all datafrom response in response.Result, we deserialize it, convert it to 
            //    //List of VillaDto type and assign it to our list
            //    list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
            //}

            VillaCreateDto dto = new VillaCreateDto();

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(dto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa created successfully!";
                    return RedirectToAction("IndexVilla");
                }
            }

            TempData["error"] = "Error encountered!!";

            return View(dto);
        }


        public async Task<IActionResult> UpdateVilla(int id)
        {
            var resp = await _villaService.GetAsync<APIResponse>(id);

            if(resp != null && resp.IsSuccess)
            {
                VillaDto villaDto = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(resp.Result));
                return View(_mapper.Map<VillaUpdateDto>(villaDto));
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Villa updated successfully!";

                var response = await _villaService.UpdateAsync<APIResponse>(dto);

                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("IndexVilla");
                }
            }

            TempData["error"] = "Error encountered!!";

            return View(dto);
        }
       
        public async Task<IActionResult> DeleteVilla(int id)
        {
            var response = await _villaService.DeleteAsync<APIResponse>(id);

            
            if (response !=null && response.IsSuccess)
            {
                TempData["success"] = "Villa deleted successfully!";

                return RedirectToAction("IndexVilla");
            }

            TempData["error"] = "Error encountered!!";
         
            return NotFound();
        }
    }
}
