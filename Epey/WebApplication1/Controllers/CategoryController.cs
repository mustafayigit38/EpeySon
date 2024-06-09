using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    [Authorize]
    public class CategoryController : Controller
    {
        private readonly EpeyContext epeyContext;

        public CategoryController()
        {
            epeyContext = new EpeyContext();
        }

        public IActionResult Index()
        {
            var categories = epeyContext.Categories.ToList();

            return View(categories);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create([FromForm] Category model)
        {

            await epeyContext.Categories.AddAsync(
                new()
                {
                    Name = model.Name,

                }
            );

            await epeyContext.SaveChangesAsync();



            return View(model);
        }
    }
}
