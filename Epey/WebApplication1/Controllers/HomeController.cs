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
			var phones = epeyContext.Phones.OrderByDescending(p => p.CreatedAt).Take(3).ToList(); // Veritaban?ndan olu?turulma tarihine g�re en yeni 3 telefonu getirir

			return View(phones); // G�r�n�me telefonlar? g�nderir
		}
	}
}
