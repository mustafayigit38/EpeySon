using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class AyakkabıController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ErkekAyakkabı()
        {
            return View();
        }
        public IActionResult KadınAyakkabı()
        {
            return View();
        }
        public IActionResult CocukAyakkabı()
        {
            return View();
        }
        public IActionResult UnisexAyakkabı()
        {
            return View();
        }
    }
}
