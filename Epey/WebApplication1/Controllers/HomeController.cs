using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly EpeyContext epeyContext;


        public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
            epeyContext = new EpeyContext();
        }

		public async Task<IActionResult> Index()
		{
            var phones = epeyContext.Phones.ToList();
            return View(phones);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
