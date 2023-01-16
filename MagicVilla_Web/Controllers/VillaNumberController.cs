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
                else
                {
                    if(resp.ErrorMessages.Count>0)
                    {
                        ModelState.AddModelError("CustomError", resp.ErrorMessages.FirstOrDefault());
                    }
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
            VillaNumberUpdateVM villaNumberUpdateVM = new VillaNumberUpdateVM();

            var resp = await _villaService.GetAllAsync<APIResponse>();

            if (resp != null && resp.IsSuccess)
            {
                villaNumberUpdateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(resp.Result))
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name,
                    });
            }

            var getVillaNumber = await _villaNumberService.GetAsync<APIResponse>(id);

            if (getVillaNumber != null && getVillaNumber.IsSuccess)
            {
                VillaNumberDto villaNumDto = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(getVillaNumber.Result));
                villaNumberUpdateVM.VillaNumber  = _mapper.Map<VillaNumberUpdateDto>(villaNumDto);

                return View(villaNumberUpdateVM);

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
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM villaNumberUpdateVM)
        {
            if (ModelState.IsValid)
            {
                var resp = await _villaNumberService.UpdateAsync<APIResponse>(villaNumberUpdateVM.VillaNumber);

                if (resp != null && resp.IsSuccess)
                {
                    return RedirectToAction("IndexVillaNumber");
                }
                else
                {
                    if(resp.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", resp.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            var allVillas = await _villaService.GetAllAsync<APIResponse>();

            if (allVillas != null && allVillas.IsSuccess)
            {
                villaNumberUpdateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(allVillas.Result))
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name,
                    });
            }


            return View(villaNumberUpdateVM);
        }

        public async Task<IActionResult> DeleteVillaNumber(int id)
        {
            var response = await _villaNumberService.DeleteAsync<APIResponse>(id);


            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }



            return NotFound();
        }
    }
}
