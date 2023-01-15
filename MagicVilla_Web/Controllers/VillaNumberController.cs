using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

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
          
            VillaNumberCreateVM villaNumberCreateVM = new VillaNumberCreateVM();

            var resp = await _villaService.GetAllAsync<APIResponse>();

            if(resp !=null && resp.IsSuccess)
            {
                villaNumberCreateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(resp.Result))
                    .Select(x=> new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name,
                    });
            }

            //var villaSelectist = villaList.Select(s => new { s.Id, Name = s.Name }).ToList();

            return View(villaNumberCreateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM villaNumberCreateVM)
        {
            if(ModelState.IsValid)
            {
                var resp = await _villaNumberService.CreateAsync<APIResponse>(villaNumberCreateVM.VillaNumber);

                if (resp != null && resp.IsSuccess)
                {
                    return RedirectToAction("IndexVillaNumber");
                }
            }

            //when MoelState not valid, repopulate villa list
            var allVillas = await _villaService.GetAllAsync<APIResponse>();

            if (allVillas != null && allVillas.IsSuccess)
            {
                villaNumberCreateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(allVillas.Result))
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name,
                    });
            }

            return View(villaNumberCreateVM);
        }


       // [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVillaNumber(int id)
        {
            

            var getVillaNumber = await _villaNumberService.GetAsync<APIResponse>(id);


            if (getVillaNumber != null && getVillaNumber.IsSuccess)
            {
                VillaNumberDto villaNumDto = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(getVillaNumber.Result));

                List<VillaDto> villaList = new();

                var resp = await _villaService.GetAllAsync<APIResponse>();
                villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(resp.Result));

                var villaSelectist = villaList.Select(s => new { s.Id, Name = s.Name }).ToList();

                VillaNumberUpdateDto villaNumberUpdateDto = new VillaNumberUpdateDto();
                villaNumberUpdateDto = _mapper.Map<VillaNumberUpdateDto>(villaNumDto);

                villaNumberUpdateDto.VillaList = new SelectList(villaSelectist, "Id", "Name");

                return View(villaNumberUpdateDto);
            }

            return NotFound();

            //_response.Result = _mapper.Map<VillaDto>(villa);

            //var villaNum = _mapper.Map<VillaNumberDto>(getVillaNumber.Result);

            //VillaNumberUpdateDto dto = new VillaNumberUpdateDto
            //{
            //    VillaList = new SelectList(villaSelectist, "Id", "Name"),
            //    VillaNo = villaNum.VillaNo
            //};

            //return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                var resp = await _villaNumberService.UpdateAsync<APIResponse>(dto);

                if (resp != null && resp.IsSuccess)
                {
                    return RedirectToAction("IndexVillaNumber");
                }
            }


            return View(dto);
        }

        public async Task<IActionResult> DeleteVillaNumber(int id)
        {
            var response = await _villaNumberService.DeleteAsync<APIResponse>(id);


            if (response != null && response.IsSuccess)
            {
                return RedirectToAction("IndexVillaNumber");
            }



            return NotFound();
        }
    }
}
