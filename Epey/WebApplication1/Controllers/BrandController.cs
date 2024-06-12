using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.Rendering; 
using WebApplication1.Models; 

namespace WebApplication1.Controllers 
{
	[Authorize] // Bu denetleyicinin tüm aksiyonlarının yetkilendirme gerektirdiğini belirtir
	public class BrandController : Controller // BrandController sınıfı, Controller sınıfından türetilmiş
	{
		private readonly EpeyContext epeyContext; // Veritabanı bağlantısını tutan değişken

		public BrandController() // Constructor
		{
			epeyContext = new EpeyContext(); // Veritabanı bağlantısını başlatır
		}

		public IActionResult Index() // Markaları listelemek için aksiyon
		{
			var brands = epeyContext.Brands.ToList(); // Veritabanından tüm markaları getirir

			return View(brands); // Görünüme markaları gönderir
		}

		public IActionResult Create() // Yeni marka oluşturma formunu getiren aksiyon
		{
			return View(); // Görünümü döner
		}

		[HttpPost, ActionName("Create")] // HTTP POST işlemi ve aksiyon adı belirleme
		public async Task<IActionResult> Create([FromForm] Brand model) // Yeni marka oluşturma aksiyonu
		{
			await epeyContext.Brands.AddAsync( // Yeni markayı veritabanına ekler
				new()
				{
					Name = model.Name, // Marka ismini modelden alır
				}
			);

			await epeyContext.SaveChangesAsync(); // Veritabanına değişiklikleri kaydeder

			return RedirectToAction("Create", "Brand"); // Yeni marka oluşturma sayfasına geri döner
		}
	}
}
