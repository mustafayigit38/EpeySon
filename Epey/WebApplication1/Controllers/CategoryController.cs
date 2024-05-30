using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{

    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IWriteRepository<Category> _categoryWriteRepository;
        private readonly IReadRepository<Category> _categoryReadRepository;

        public CategoryController(IWriteRepository<Category> categoryWriteRepository, IReadRepository<Category> categoryReadRepository)
        {
            _categoryWriteRepository = categoryWriteRepository;
            _categoryReadRepository = categoryReadRepository;
        }

        public IActionResult Index()
        {
            var categories = _categoryReadRepository.GetAll();

            return View(categories);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create([FromForm] Category model)
        {

            await _categoryWriteRepository.AddAsync(
                new()
                {
                    Name = model.Name,

                }
            );

            await _categoryWriteRepository.SaveAsync();



            return View(model);
        }
    }
}
