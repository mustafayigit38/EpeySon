using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using System.Net; 
using WebApplication1.Models; 

namespace WebApplication1.Controllers // Namespace belirlemesi
{
	[Authorize] // Bu denetleyicinin tüm aksiyonlarının yetkilendirme gerektirdiğini belirtir
	public class CategoryController : Controller // CategoryController sınıfı, Controller sınıfından türetilmiş
	{
		private readonly EpeyContext epeyContext; // Veritabanı bağlantısını tutan değişken

		public CategoryController() // Constructor
		{
			epeyContext = new EpeyContext(); // Veritabanı bağlantısını başlatır
		}

		public IActionResult Index() // Kategorileri listelemek için aksiyon
		{
			var categories = epeyContext.Categories.ToList(); // Veritabanından tüm kategorileri getirir

			return View(categories); // Görünüme kategorileri gönderir
		}

		public IActionResult Create() // Yeni kategori oluşturma formunu getiren aksiyon
		{
			return View(); // Görünümü döner
		}

		[HttpPost, ActionName("Create")] // HTTP POST işlemi ve aksiyon adı belirleme
		public async Task<IActionResult> Create([FromForm] Category model) // Yeni kategori oluşturma aksiyonu
		{
			await epeyContext.Categories.AddAsync( // Yeni kategoriyi veritabanına ekler
				new()
				{
					Name = model.Name, // Kategori ismini modelden alır
				}
			);

			await epeyContext.SaveChangesAsync(); // Veritabanına değişiklikleri kaydeder

			return View(model); // Oluşturulan kategori ile birlikte görünümü döner
		}
	}
}
