using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.LoginAsync<APIResponse>(dto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Login was successful!";
                    return RedirectToAction("Login");
                }
            }

            TempData["error"] = "Error encountered!!";

            return View(dto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegistrationRequestDto obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.RegisterAsync<APIResponse>(dto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Registration was successful!";
                    return RedirectToAction("Login");
                }
            }

            TempData["error"] = "Error encountered!!";

            return View(dto);
        }

        public async Task<IActionResult> Logout()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
