using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

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
             APIResponse response = await _authService.LoginAsync<APIResponse>(dto);

            if (response != null && response.IsSuccess)
            {
                LoginResponseDto responseObj = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

                //so the httpcontext knows that user is logged in - actually signing in a user
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, responseObj.User.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, responseObj.User.Role));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                //now user is singed together with claims

                //we retrieve token from responseObj and store it in session
                HttpContext.Session.SetString(StaticDetails.SessionToken, responseObj.Token);


                TempData["success"] = "Login was successful!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
                return View(dto);
            }
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
            
            var response = await _authService.RegisterAsync<APIResponse>(dto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Registration was successful!";
                return RedirectToAction("Login");
            }
            

            TempData["error"] = "Error encountered!!";

            return View(dto);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(StaticDetails.SessionToken, "");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
