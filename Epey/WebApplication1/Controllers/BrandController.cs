using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        private readonly IWriteRepository<Brand> _brandWriteRepository;
        private readonly IReadRepository<Brand> _brandReadRepository;
        private readonly IReadRepository<Category> _categoryReadRepository;

        public BrandController(IWriteRepository<Brand> brandWriteRepository, IReadRepository<Brand> brandReadRepository, IReadRepository<Category> categoryReadRepository)
        {
            _brandWriteRepository = brandWriteRepository;
            _brandReadRepository = brandReadRepository;
            _categoryReadRepository = categoryReadRepository;
        }

        public IActionResult Index()
        {
            var brands = _brandReadRepository.GetAll();

            return View(brands);
        }

        public IActionResult Create()
        {
            

            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create([FromForm] Brand model)
        {

            await _brandWriteRepository.AddAsync(
                new()
                {
                    Name = model.Name,

                }
            );

            await _brandWriteRepository.SaveAsync();


            return RedirectToAction("Create","Brand");
        }
    }
}
