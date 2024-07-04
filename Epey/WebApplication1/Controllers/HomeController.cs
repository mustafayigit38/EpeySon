using Microsoft.AspNetCore.Mvc; 
using System.Diagnostics; 
using WebApplication1.Models; 

namespace WebApplication1.Controllers 
{
	public class HomeController : Controller 
	{
		private readonly EpeyContext epeyContext; // Veritaban? ba?lant?s?n? tutan de?i?ken

		public HomeController() // Constructor
		{
			epeyContext = new EpeyContext(); // Veritaban? ba?lant?s?n? ba?lat?r
		}

		public async Task<IActionResult> Index() // Anasayfa aksiyonu
		{
			var phones = epeyContext.Phones.OrderByDescending(p => p.CreatedAt).ToList(); // Veritaban?ndan olu?turulma tarihine göre en yeni 3 telefonu getirir

            // Fetch all products, then sort and parse in memory
            var products = epeyContext.Products.ToList(); // Load into memory
            products = products.OrderByDescending(p => DateTime.Parse(p.ProductCreatedAt)).ToList();

            return View(products); // Görünüme telefonlar? gönderir
		}
	}
}
//resimler iki kere geliyor