using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService, IMapper mapper)
        {
            _villaNumberService = villaNumberService;
            _villaService = villaService;   
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDto> listVillaNumbers = new();

            var resp = await _villaNumberService.GetAllAsync<APIResponse>();

            if (resp != null && resp.IsSuccess)
            {
                listVillaNumbers = JsonConvert.DeserializeObject<List<VillaNumberDto>>(Convert.ToString(resp.Result));
            }

            return View(listVillaNumbers);
        }

        public async Task<IActionResult> CreateVillaNumber()
        {
            List<VillaDto> villaList = new();

            var resp = await _villaService.GetAllAsync<APIResponse>();
            villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(resp.Result));

            var villaSelectist = villaList.Select(s => new { s.Id, Name = s.Name }).ToList();

            VillaNumberCreateDto dto = new VillaNumberCreateDto
            {

                VillaList = new SelectList(villaSelectist, "Id", "Name")
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateDto dto)
        {
            if(ModelState.IsValid)
            {
                var resp = await _villaNumberService.CreateAsync<APIResponse>(dto);

                if (resp != null && resp.IsSuccess)
                {
                    return RedirectToAction("IndexVillaNumber");
                }
            }
            

            return View(dto);
        }
    }
}
