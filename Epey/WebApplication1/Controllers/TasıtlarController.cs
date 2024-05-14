using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class TasıtlarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Araba()
        {
            return View();
        }
        public IActionResult Motosiklet()
        {
            return View();
        }
        public IActionResult Bisiklet()
        {
            return View();
        }
    }
}
