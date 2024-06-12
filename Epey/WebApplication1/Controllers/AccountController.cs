using Microsoft.AspNetCore.Authentication.Cookies; 
using Microsoft.AspNetCore.Authentication; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Models; 
using System.Net; 

namespace WebApplication1.Controllers 
{
	public class AccountController : Controller // AccountController sınıfı, Controller sınıfından türetilmiş
	{
		private readonly EpeyContext epeyContext; // Veritabanı bağlantısını tutan değişken

		public AccountController() // Constructor
		{
			epeyContext = new EpeyContext(); // Veritabanı bağlantısını başlatır
		}

		[HttpPost, ActionName("Register")] // HTTP POST işlemi ve aksiyon adı belirleme
		public async Task<IActionResult> Register() // Kullanıcı kayıt aksiyonu
		{
			await epeyContext.Users.AddAsync(new User() { Email = "admin@admin.com", Password = "password" }); // Yeni kullanıcıyı veritabanına ekler
			await epeyContext.SaveChangesAsync(); // Veritabanına değişiklikleri kaydeder
			return StatusCode((int)HttpStatusCode.Created); // HTTP 201 Created durum kodunu döner
		}

		[HttpGet] // HTTP GET işlemi belirleme
		public IActionResult Login(string returnUrl = null) // Kullanıcı giriş formunu getiren aksiyon
		{
			ViewData["ReturnUrl"] = returnUrl; // Geri dönülecek URL'yi ViewData'ya ekler
			return View(); // Görünümü döner
		}

		[HttpPost] // HTTP POST işlemi belirleme
		public async Task<IActionResult> Login([FromForm] User userData, string returnUrl = null) // Kullanıcı giriş aksiyonu
		{
			ViewData["ReturnUrl"] = returnUrl; // Geri dönülecek URL'yi ViewData'ya ekler
			if (userData == null) // Kullanıcı verisi yoksa görünümü döner
			{
				return View();
			}

			if (!ModelState.IsValid) // Model geçerli değilse kullanıcı verisiyle birlikte görünümü döner
			{
				return View(userData);
			}

			User user = await epeyContext.Users.FirstOrDefaultAsync(u => u.Email == userData.Email && u.Password == userData.Password); // Veritabanından kullanıcıyı bulur

			if (user is null) // Kullanıcı bulunamazsa kullanıcı verisiyle birlikte görünümü döner
			{
				return View(userData);
			}

			var claims = new List<Claim> { // Kullanıcı için iddia (claim) listesi oluşturur
                new Claim(ClaimTypes.Name, userData.Email),
			};

			var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); // Kimlik doğrulama şemasıyla yeni kimlik oluşturur
			var claimPrincipal = new ClaimsPrincipal(claimIdentity); // Yeni bir kimlik doğrulama prensi oluşturur
			await HttpContext.SignInAsync(claimPrincipal); // Kullanıcıyı oturum açmış olarak işaretler

			if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) // Geri dönülecek URL varsa ve yerel bir URL ise oraya yönlendirir
			{
				return Redirect(returnUrl);
			}
			else // Yoksa ana sayfaya yönlendirir
			{
				return RedirectToAction("Index", "Home");
			}
		}

		public IActionResult AccessDenied() // Erişim reddedildiğinde gösterilecek aksiyon
		{
			return View(); // Görünümü döner
		}
	}
}
