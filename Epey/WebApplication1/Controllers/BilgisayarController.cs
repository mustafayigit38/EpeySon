using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class BilgisayarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dizüstü()
        {
            return View();
        }
        public IActionResult Masaüstü()
        {
            return View();
        }
    }
}
