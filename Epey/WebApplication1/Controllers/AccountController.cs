using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Repositories;
using WebApplication1.Models;
using System.Net;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly IReadRepository<User> _userReadRepository;
        private readonly IWriteRepository<User> _userWriteRepository;

        public AccountController(IReadRepository<User> userReadRepository, IWriteRepository<User> userWriteRepository)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
        }

        /*
        [HttpPost, ActionName("Register")]
        public async Task<IActionResult> Register()
        {
            await _userWriteRepository.AddAsync(
                 new User() { Email = "admin@admin.com", Password = "password" }
                 );
            await _userWriteRepository.SaveAsync();
            //return created status code
            return StatusCode((int)HttpStatusCode.Created);
        }*/

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] User userData, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (userData == null)
            {
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View(userData);
            }

            User user = await _userReadRepository.GetAll().FirstOrDefaultAsync(u => u.Email == userData.Email && u.Password == userData.Password);

            if (user is null)
            {
                return View(userData);
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name,userData.Email),
            };
            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync(claimPrincipal);


            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
