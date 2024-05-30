using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using WebApplication1.Models;
using WebApplication1.Models.PhoneSpecs.PhoneBattery;
using WebApplication1.Models.PhoneSpecs.PhoneScreen;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class PhoneController : Controller
    {
        private readonly IWriteRepository<Brand> _brandWriteRepository;
        private readonly IReadRepository<Brand> _brandReadRepository;
        private readonly IReadRepository<Category> _categoryReadRepository;
        private readonly IWriteRepository<Phone> _phoneWriteRepository;
        private readonly IReadRepository<Phone> _phoneReadRepository;

        IWebHostEnvironment _webHostEnvironment;

        public PhoneController(IWriteRepository<Brand> brandWriteRepository, IReadRepository<Brand> brandReadRepository, IReadRepository<Category> categoryReadRepository, IWriteRepository<Phone> phoneWriteRepository, IReadRepository<Phone> phoneReadRepository, IWebHostEnvironment webHostEnvironment)
        {
            _brandWriteRepository = brandWriteRepository;
            _brandReadRepository = brandReadRepository;
            _categoryReadRepository = categoryReadRepository;
            _phoneWriteRepository = phoneWriteRepository;
            _phoneReadRepository = phoneReadRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var brands = _phoneReadRepository.GetAll().ToList();

            return View(brands);
        }

        public IActionResult Create()
        {
            var categories = _categoryReadRepository.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            var brands = _brandReadRepository.GetAll().Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            });

            var enums = Enum.GetValues(typeof(PhoneScreenFeatureEnum)).Cast<PhoneScreenFeatureEnum>().Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.ToString()
            });

            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            ViewBag.Enums = enums;

            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create([FromForm] PhoneRequest_VM model)
        {
           
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "photos");


            string filePath = Path.Combine(uploadPath, model.PhotoFile.FileName);
            try
            {
                await using FileStream fileStream =
                new(
                        filePath,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None,
                        1024 * 1024,
                        useAsync: false
                    );

                await model.PhotoFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
               
            }
            catch (Exception er)
            {
                throw er;
            }

        await _phoneWriteRepository.AddAsync(
                new()
                {
                    Name = model.Name,
                    Brand = model.Brand,
                    Category = model.Category,
                    Price = model.Price,
                    Seller = model.Seller,
                    ImagePath = "photos/" + model.PhotoFile.FileName,
                    PhoneScreenSpecs = new()
                    {
                        ScreenResolution = model.PhoneScreenSpecs.ScreenResolution,
                        ScreenSize = model.PhoneScreenSpecs.ScreenSize,
                        ScreenFeature = model.PhoneScreenSpecs.ScreenFeature,
                        ScreenRefreshRate = model.PhoneScreenSpecs.ScreenRefreshRate
                    },
                    PhoneCameraSpecs = new()
                    {
                        CameraResolution = model.PhoneCameraSpecs.CameraResolution,
                        CameraFps = model.PhoneCameraSpecs.CameraFps,
                        CameraZoom = model.PhoneCameraSpecs.CameraZoom
                    },
                    PhoneBatterySpecs = new()
                    {
                        BatteryCapacity = model.PhoneBatterySpecs.BatteryCapacity,
                        FastCharging = model.PhoneBatterySpecs.FastCharging,
                        ChargingSpeed = model.PhoneBatterySpecs.ChargingSpeed
                    }





                }
            );

            await _brandWriteRepository.SaveAsync();


            return RedirectToAction("Create", "Phone");
        }

        public IActionResult Compare(string url)
        {
            if (url == null)
            {
                return NotFound();
            }

            var phoneIds = url.Split("-");

            var phones = _phoneReadRepository.GetAll().Where(p => phoneIds.Contains(p.Id.ToString())).ToList();


            return View(phones);
        }
    }
}
