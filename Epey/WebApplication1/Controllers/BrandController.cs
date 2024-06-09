using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        private readonly EpeyContext epeyContext;

        public BrandController()
        {
            epeyContext = new EpeyContext();
        }

        public IActionResult Index()
        {
            var brands = epeyContext.Brands.ToList();

            return View(brands);
        }

        public IActionResult Create()
        {
            

            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create([FromForm] Brand model)
        {

            await epeyContext.Brands.AddAsync(
                new()
                {
                    Name = model.Name,

                }
            );

            await epeyContext.SaveChangesAsync();


            return RedirectToAction("Create","Brand");
        }
    }
}
