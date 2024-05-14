using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class TelefonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AkıllıTelefon() 
        {
            return View();
        }
        public IActionResult TusluTelefon ()
        {
            return View();
        }
        public IActionResult AkıllıSaat()
        {
            return View();
        }
        public IActionResult SarjAleti()
        {
            return View();
        }

    }
}
