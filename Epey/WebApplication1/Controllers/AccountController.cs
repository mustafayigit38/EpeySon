using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly EpeydbContext _context;

        public AccountController()
        {
            _context = new EpeydbContext();
        }
        private string Username { get { return "ali"; } }
        private string Password { get { return "1234"; } }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] MyUser userData)
        {
            if (userData == null)
            {
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View(userData);
            }

            var user = _context.Kullanıcılar.FirstOrDefault(x => x.İsim == userData.Username && x.Sifre == userData.Password);

            if (user is null)
            {
                return View(userData);
            }

            /*** Oturum Açma işlemleri başladı ****/
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name,userData.Username),
                new Claim("OturumAçmaZamanı",DateTime.Now.ToString("dd MM yyyy hh:mm:ss"))
            };
            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync(claimPrincipal);
            await Console.Out.WriteLineAsync("Kullanıcı oturum açtı");
            return RedirectToAction("Index", "Home");
            /*** Oturum Açma işlemleri bitti ****/


        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
