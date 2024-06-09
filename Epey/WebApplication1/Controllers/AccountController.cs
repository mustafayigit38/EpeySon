using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Models;
using System.Net;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly EpeyContext epeyContext;

        public AccountController()
        {
            epeyContext = new EpeyContext();
        }



        [HttpPost, ActionName("Register")]
        public async Task<IActionResult> Register()
        {
            await epeyContext.Users.AddAsync(new User() { Email = "admin@admin.com", Password = "password" });
            await epeyContext.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }

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

            User user = await epeyContext.Users.FirstOrDefaultAsync(u => u.Email == userData.Email && u.Password == userData.Password);

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
