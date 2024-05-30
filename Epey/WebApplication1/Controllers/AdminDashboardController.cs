using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
