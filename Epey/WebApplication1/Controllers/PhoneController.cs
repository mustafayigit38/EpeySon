using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApplication1.Models;
using WebApplication1.ViewModels;


namespace WebApplication1.Controllers
{
    public class PhoneController : Controller // PhoneController sınıfı, Controller sınıfından türetilmiş
    {
        IWebHostEnvironment _webHostEnvironment; // Web barındırma ortamını tutan değişken
        private readonly EpeyContext epeyContext; // Veritabanı bağlantısını tutan değişken

        public PhoneController(IWebHostEnvironment webHostEnvironment) // Constructor, IWebHostEnvironment parametresi alır
        {
            _webHostEnvironment = webHostEnvironment; // Web barındırma ortamını değişkene atar
            epeyContext = new EpeyContext(); // Veritabanı bağlantısını başlatır
        }

        public IActionResult Phonebybrand(string brand) // Belirli bir markanın telefonlarını listelemek için aksiyon
        {
            var brands = epeyContext.Phones.Include(p => p.Brand).Where(p => p.Brand.Name == brand).ToList(); // Veritabanından markaya göre telefonları getirir
            return View("Index", brands); // "Index" görünümüne markaya ait telefonları gönderir
        }

        public IActionResult Index() // Tüm telefonları listelemek için aksiyon
        {
            var brands = epeyContext.Phones.ToList(); // Veritabanından tüm telefonları getirir
            return View(brands); // Görünüme telefonları gönderir
        }

        public IActionResult GetPhone(string id) // Belirli bir telefonun detaylarını almak için aksiyon
        {
            var phone = epeyContext.Phones.FirstOrDefault(p => p.Id.ToString() == id); // Veritabanından ID'ye göre telefonu bulur
            var comments = epeyContext.Comments.Where(c => c.ProductId == phone.Id).ToList(); // Telefona ait yorumları getirir

            GetPhone_VM getPhone_VM = new GetPhone_VM // ViewModel'i oluşturur ve doldurur
            {
                Phone = phone,
                Comment = comments
            };

            return View(getPhone_VM); // Görünüme ViewModel'i gönderir
        }

        public IActionResult SendComment(Comment com, Phone phone) 
        {
            var comment = epeyContext.Comments.Add(new Comment 
            {
                ProductId = phone.Id,
                Text = com.Text
            });

            epeyContext.SaveChanges(); 

            return RedirectToAction("GetPhone", "Phone", new { id = phone.Id.ToString() }); 
        }

        [Authorize] // Yetkilendirme gerektiren aksiyon
        public IActionResult Create() // Yeni telefon oluşturma formunu getiren aksiyon
        {
            var categories = epeyContext.Categories.Select(c => new SelectListItem // Kategorileri seçme listesi için hazırlar
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            var brands = epeyContext.Brands.Select(b => new SelectListItem // Markaları seçme listesi için hazırlar
            {
                Value = b.Id.ToString(),
                Text = b.Name
            });

            ViewBag.Categories = categories; // Kategorileri ViewBag'e ekler
            ViewBag.Brands = brands; // Markaları ViewBag'e ekler

            return View(); // Görünümü döner
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

        public IActionResult Compare() // Karşılaştırma işlemi için aksiyon
        {
            if (CompareItems.CompareItemId.Count == 0) // Karşılaştırma listesi boşsa ana sayfaya yönlendirir
            {
                return RedirectToAction("Index", "Home");
            }

            var phones = epeyContext.Phones.Where(p => CompareItems.CompareItemId.Contains(p.Id.ToString())).ToList(); // Karşılaştırma listesine göre telefonları getirir
            CompareItems.CompareItemId.Clear(); // Karşılaştırma listesini temizler

            return View(phones); // Karşılaştırma görünümüne telefonları gönderir
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
