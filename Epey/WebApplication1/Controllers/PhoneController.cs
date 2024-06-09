using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Net;
using WebApplication1.Models;
using WebApplication1.ViewModels;

using WebApplication1.Models.PhoneSpecs.PhoneScreen;

namespace WebApplication1.Controllers
{
    public class PhoneController : Controller
    {
        IWebHostEnvironment _webHostEnvironment;
        private readonly EpeyContext epeyContext;


        public PhoneController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            epeyContext = new EpeyContext();
        }
        public IActionResult Phonebybrand(string brand)
        {
            var brands = epeyContext.Phones.Include(p => p.Brand).Where(p => p.Brand.Name == brand).ToList();



            return View("Index", brands);
        }

        public IActionResult Index()
        {

            var brands = epeyContext.Phones.ToList();



            return View(brands);
        }
        [Authorize]
        public IActionResult Create()
        {
            var categories = epeyContext.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            var brands = epeyContext.Brands.Select(b => new SelectListItem
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
        [Authorize]
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

            await epeyContext.Phones.AddAsync(
                    new()
                    {
                        Name = model.Name,
                        BrandId = model.BrandId,
                        Category = model.Category,
                        Price = model.Price,
                        Seller = model.Seller,
                        ImagePath = "photos/" + model.PhotoFile.FileName,
                        PhoneScreenSpecsScreenResolution = model.PhoneScreenSpecsScreenResolution,
                        PhoneScreenSpecsScreenSize = model.PhoneScreenSpecsScreenSize,
                        PhoneScreenSpecsScreenFeature = model.PhoneScreenSpecsScreenFeature,
                        PhoneScreenSpecsScreenRefreshRate = model.PhoneScreenSpecsScreenRefreshRate,
                        PhoneCameraSpecsCameraResolution = model.PhoneCameraSpecsCameraResolution,
                        PhoneCameraSpecsCameraFps = model.PhoneCameraSpecsCameraFps,
                        PhoneCameraSpecsCameraZoom = model.PhoneCameraSpecsCameraZoom,
                        PhoneBatterySpecsBatteryCapacity = model.PhoneBatterySpecsBatteryCapacity,
                        PhoneBatterySpecsFastCharging = model.PhoneBatterySpecsFastCharging,
                        PhoneBatterySpecsChargingSpeed = model.PhoneBatterySpecsChargingSpeed
                    }
                );

            await epeyContext.SaveChangesAsync();


            return RedirectToAction("Create", "Phone");
        }

        public IActionResult Compare()
        {
            



            var phones = epeyContext.Phones.Where(p => CompareItems.CompareItemId.Contains(p.Id.ToString())).ToList();


            return View(phones);
        }

        [HttpPost]
        public IActionResult AddToCompare([FromQuery] string id)
        {
            if (CompareItems.CompareItemId == null)
            {
                CompareItems.CompareItemId = new List<string>();
            }

            if (CompareItems.CompareItemId.Contains(id))
            {
                CompareItems.CompareItemId.Remove(id);
            }
            else
            {
                CompareItems.CompareItemId.Add(id);
            }

            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPost]
		public IActionResult CompareItemsClear()
		{
			if (CompareItems.CompareItemId.Count != 0)
			{
				CompareItems.CompareItemId.Clear();
			}
			return StatusCode((int)HttpStatusCode.Accepted);
		}


	}
}
