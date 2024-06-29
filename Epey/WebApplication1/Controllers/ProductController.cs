using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
	public class ProductController : Controller
	{
		// IWebHostEnvironment arayüzü, uygulamanın web ana bilgisayar ortamını temsil eder
		IWebHostEnvironment _webHostEnvironment;
		// EpeyContext, veritabanı işlemleri için kullanılan Entity Framework bağlamıdır
		private readonly EpeyContext epeyContext;

		// Constructor, bağımlılıkları alır ve epeyContext ve _webHostEnvironment örneklerini başlatır
		public ProductController(IWebHostEnvironment webHostEnvironment)
		{
			epeyContext = new EpeyContext();
			_webHostEnvironment = webHostEnvironment;
		}

		// Bu yöntem bir Value nesnesinin değerini döndürür
		private object GetValue(Value v)
		{
			return v.Attribute.AttributeType switch
			{
				// Eğer AttributeType "string" ise, ValueString değerini döndür
				"string" => (object)v.ValueString,
				// Eğer AttributeType "integer" ise, ValueInteger değerini döndür
				"integer" => v.ValueInteger,
				// Eğer AttributeType "float" ise, ValueFloat değerini döndür
				"float" => v.ValueFloat,
				// Eğer AttributeType "date" ise, ValueDate değerini döndür
				"date" => v.ValueDate,
				// Eğer AttributeType "boolean" ise, ValueBoolean değerini döndür
				"boolean" => v.ValueBoolean,
				// Yukarıdaki durumların hiçbiri değilse, null döndür
				_ => null
			};
		}

		// Belirtilen ID'ye sahip ürünü alır
		public async Task<IActionResult> GetProduct(int id)
		{
			// Veritabanından belirtilen ID'ye sahip ürünleri alır
			var products = await epeyContext.Products
					  .Include(x => x.ProductCategoryNavigation) // Ürün kategorisini de dahil eder
					  .Include(p => p.Values) // Ürün değerlerini de dahil eder
					  .ThenInclude(v => v.Attribute) // Değerlerin özelliklerini de dahil eder
					  .Where(p => p.ProductId == id) // Belirtilen ID'ye sahip ürünleri filtreler
					  .ToListAsync();

			// Ürün detaylarını seçer ve gruplar
			var productDetails = products.Select(x => new
			{
				ProductName = x.ProductName,
				ProductId = x.ProductId,
				ProductImage = x.ProductImage,
				Values = x.Values.Select(v => new { v.Attribute.AttributeName, Value = GetValue(v) }).GroupBy(x => x.AttributeName)
			});

			// View ile ürün detaylarını döndürür
			return View(productDetails);
		}

		// Yeni ürün kategorisi oluşturma sayfasını döndürür
		public IActionResult CreateProductCategory()
		{
			// Veritabanındaki kategorileri SelectListItem nesnelerine dönüştürür
			var productDetails = epeyContext.Categories.Select(c => new SelectListItem
			{
				Value = c.Id.ToString(),
				Text = c.Name,
			});

			// Kategorileri ViewBag ile View'e aktarır
			ViewBag.ProductCategory = productDetails;

			// CreateProductCategory View'ini döndürür
			return View();
		}

		// Tüm özellikleri listeler
		public async Task<IActionResult> ListAttributeSpecs()
		{
			// Veritabanından tüm AttributeSpec nesnelerini alır
			var specs = await epeyContext.AttributeSpecs.ToListAsync();
			// View ile özellikleri döndürür
			return View(specs);
		}

		// İki değeri karşılaştırır
		private int CompareValues(Value value1, Value value2)
		{
			// Değerlerin AttributeType özelliğine göre karşılaştırma yapar
			switch (value1.Attribute.AttributeType)
			{
				case "integer":
					// Eğer AttributeType "integer" ise, ValueInteger değerlerini karşılaştırır
					int? int1 = value1.ValueInteger;
					int? int2 = value2.ValueInteger;
					return Nullable.Compare(int1, int2);

				case "float":
					// Eğer AttributeType "float" ise, ValueFloat değerlerini karşılaştırır
					double? float1 = value1.ValueFloat;
					double? float2 = value2.ValueFloat;
					return Nullable.Compare(float1, float2);

				case "date":
					// Eğer AttributeType "date" ise, ValueDate değerlerini karşılaştırır
					DateOnly? date1 = value1.ValueDate;
					DateOnly? date2 = value2.ValueDate;
					return Nullable.Compare(date1, date2);

				case "string":
					// Eğer AttributeType "string" ise, ValueString değerlerini karşılaştırır
					string string1 = value1.ValueString;
					string string2 = value2.ValueString;
					return string.Compare(string1, string2, StringComparison.Ordinal);

				case "boolean":
					// Eğer AttributeType "boolean" ise, ValueBoolean değerlerini karşılaştırır
					int? bool1 = value1.ValueBoolean;
					int? bool2 = value2.ValueBoolean;
					return Nullable.Compare(bool1, bool2);

				default:
					// Yukarıdaki durumların hiçbiri değilse, 0 döndür
					return 0;
			}
		}

		// Ürünleri karşılaştırır
		public async Task<IActionResult> CompareProducts()
		{
			// Karşılaştırılacak ürünlerin ID'lerini alır
			var productIds = CompareItems.CompareItemId.Select(id => int.Parse(id)).ToList();
			// Veritabanından belirtilen ID'lere sahip ürünleri alır
			var products = await epeyContext.Products
				.Include(p => p.Values)
				.ThenInclude(v => v.Attribute)
				.ThenInclude(a => a.AttributeSpecNavigation)  // Ensure this is included
				.Where(p => productIds.Contains(p.ProductId))
				.ToListAsync();

			// Eğer ürün yoksa, NotFound döndürür
			if (!products.Any())
			{
				return NotFound();
			}

			// Tüm özellikleri alır
			var allAttributes = products.SelectMany(p => p.Values.Select(v => v.Attribute)).Distinct().ToList();

			// Karşılaştırma sonuçlarını tutmak için bir liste oluşturur
			var comparisonResults = new List<ComparisonResult>();

			// Her bir özellik için karşılaştırma yapar
			foreach (var attribute in allAttributes)
			{
				var comparisonResult = new ComparisonResult
				{
					AttributeName = attribute.AttributeName,
					AttributeSpecName = attribute.AttributeSpecNavigation?.AttributeSpecsName,
					ProductValues = new List<ProductAttributeValue>()
				};

				// Her bir ürün için değeri ekler
				foreach (var product in products)
				{
					var value = product.Values.FirstOrDefault(v => v.AttributeId == attribute.AttributeId);
					comparisonResult.ProductValues.Add(new ProductAttributeValue
					{
						ProductName = product.ProductName,
						ProductImage = product.ProductImage,
						Value = value != null ? GetValue(value) : null
					});
				}

				// Karşılaştırma sonucunu listeye ekler
				comparisonResults.Add(comparisonResult);
			}

			// Karşılaştırma listelerini temizler
			CompareItems.CompareItemId.Clear();

			// View ile karşılaştırma sonuçlarını döndürür
			return View(comparisonResults);
		}

		// Yeni özellik oluşturma sayfasını döndürür
		public IActionResult CreateAttributeSpecs()
		{
			return View();
		}

		// Yeni özellik oluşturma işlemini yapar
		[HttpPost]
		public async Task<IActionResult> CreateAttributeSpecs(AttributeSpec attributeSpec)
		{
			// Yeni AttributeSpec nesnesini veritabanına ekler
			var test = await epeyContext.AttributeSpecs.AddAsync(new AttributeSpec
			{
				AttributeSpecsName = attributeSpec.AttributeSpecsName,
			});

			// Değişiklikleri veritabanına kaydeder
			var test2 = await epeyContext.SaveChangesAsync();
			// ListAttributeSpecs eylemine yönlendirir
			return RedirectToAction(nameof(ListAttributeSpecs));
		}

		// Yeni özellik oluşturma sayfasını döndürür
		public IActionResult CreateAttribute()
		{
			// Kategorileri SelectListItem nesnelerine dönüştürür
			var category = epeyContext.Categories.Select(c => new SelectListItem
			{
				Value = c.Id.ToString(),
				Text = c.Name
			});

			// Özellikleri SelectListItem nesnelerine dönüştürür
			var specs = epeyContext.AttributeSpecs.Select(s => new SelectListItem
			{
				Value = s.AttributeSpecsId.ToString(),
				Text = s.AttributeSpecsName
			});

			// Kategori ve özellikleri ViewBag ile View'e aktarır
			ViewBag.Category = category;
			ViewBag.Specs = specs;

			// CreateAttribute View'ini döndürür
			return View();
		}

		// Yeni özellik oluşturma işlemini yapar
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateAttribute(CreateAttribute_VM createAttribute_VM)
		{
			int attributeId = 0;
			// Yeni Attribute modelini oluşturur
			var attirbuteModel = new Models.Attribute
			{
				AttributeName = createAttribute_VM.attribute.AttributeName,
				AttributeType = createAttribute_VM.attribute.AttributeType,
				AttributeSpec = createAttribute_VM.attributeSpec.AttributeSpecsId
			};

			// Aynı özellik daha önce oluşturulmuş mu kontrol eder
			var isAttributeExist = epeyContext.Attributes.FirstOrDefault(x =>
				x.AttributeName == attirbuteModel.AttributeName &&
				x.AttributeSpec == attirbuteModel.AttributeSpec &&
				x.AttributeType == attirbuteModel.AttributeType
			);

			// Eğer özellik zaten varsa, ID'sini alır
			if (isAttributeExist != null)
			{
				attributeId = isAttributeExist.AttributeId;
			}
			else
			{
				// Eğer özellik yoksa, yeni özellik oluşturur ve veritabanına ekler
				var context = await epeyContext.Attributes.AddAsync(attirbuteModel);
				await epeyContext.SaveChangesAsync();
				attributeId = context.Entity.AttributeId;
			}

			// Kategoriyi veritabanından bulur
			var category = epeyContext.Categories.Find(createAttribute_VM.categories.Id);

			// Eğer kategori ve özellik varsa, kategori ile özelliği ilişkilendirir
			if (category != null && attirbuteModel != null)
			{
				epeyContext.CategoryAttributes.Add(new CategoryAttribute
				{
					AttributeId = attributeId,
					CategoryId = category.Id
				});
			}

			// Değişiklikleri veritabanına kaydeder
			await epeyContext.SaveChangesAsync();

			// CreateAttribute eylemine yönlendirir
			return RedirectToAction(nameof(CreateAttribute));
		}

		// Yeni ürün oluşturma sayfasını döndürür
		public async Task<IActionResult> CreateProduct(int id)
		{
			// Belirtilen kategoriye ait özellikleri alır
			var attributes = await epeyContext.Attributes
				.Include(x => x.CategoryAttributes)
				.Where(x => x.CategoryAttributes.Any(ca => ca.CategoryId == id))
				.ToListAsync();

			// Yeni ürün modelini oluşturur
			var model = new CreateProductViewModel
			{
				ProductCategory = id,
				Values = attributes.Select(a => new AttributeValueViewModel
				{
					AttributeID = a.AttributeId,
					AttributeName = a.AttributeName,
					AttributeType = a.AttributeType,
				}).ToList()
			};
			// CreateProduct View'ini döndürür
			return View(model);
		}

		// Ürün kategorilerini listeleme sayfasını döndürür
		public IActionResult CreateProductList()
		{
			// Veritabanındaki kategorileri alır
			var categories = epeyContext.Categories.ToList();
			// CreateProductList View'ini döndürür
			return View(categories);
		}

		// Yeni ürün oluşturma işlemini yapar
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
		{
			// Dosya yükleme yolunu belirler
			string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "photos");
			// Dosya yolunu belirler
			string filePath = Path.Combine(uploadPath, model.ProductImage.FileName);

			try
			{
				// Dosyayı asenkron olarak kaydeder
				await using FileStream fileStream = new FileStream(
						filePath,
						FileMode.Create,
						FileAccess.Write,
						FileShare.None,
						1024 * 1024,
						useAsync: false
					);

				await model.ProductImage.CopyToAsync(fileStream);
				await fileStream.FlushAsync();
			}
			catch (Exception er)
			{
				throw er;
			}

			// Yeni ürün modelini oluşturur
			var product = new Product
			{
				ProductName = model.ProductName,
				ProductCategory = model.ProductCategory,
				ProductCreatedAt = DateTime.Now.ToString(),
				ProductImage = "photos/" + model.ProductImage.FileName,
				Values = model.Values.Select(v => new Value
				{
					AttributeId = v.AttributeID,
					ValueString = v.ValueString,
					ValueInteger = v.ValueInteger,
					ValueFloat = v.ValueFloat,
					ValueDate = v.ValueDate,
					ValueBoolean = v.ValueBoolean
				}).ToList()
			};

			// Yeni ürünü veritabanına ekler
			epeyContext.Products.Add(product);
			await epeyContext.SaveChangesAsync();
			// CreateProduct eylemine yönlendirir
			return RedirectToAction(nameof(CreateProduct), new { id = model.ProductCategory });
		}
	}
}
