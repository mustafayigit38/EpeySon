using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class TelefonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AkilliTelefon() 
        {
            return View();
        }
        public IActionResult TusluTelefon ()
        {
            return View();
        }
        public IActionResult AkilliSaat()
        {
            return View();
        }
        public IActionResult SarjAleti()
        {
            return View();
        }

    }
}
