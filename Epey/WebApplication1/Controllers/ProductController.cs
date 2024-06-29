using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Xml.Linq;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        IWebHostEnvironment _webHostEnvironment;
        private readonly EpeyContext epeyContext;

        public ProductController(IWebHostEnvironment webHostEnvironment)
        {

            epeyContext = new EpeyContext();
            _webHostEnvironment = webHostEnvironment;
        }

        private object GetValue(Value v)
        {
            return v.Attribute.AttributeType switch
            {
                "string" => (object)v.ValueString,
                "integer" => v.ValueInteger,
                "float" => v.ValueFloat,
                "date" => v.ValueDate,
                "boolean" => v.ValueBoolean,
                _ => null
            };
        }

        public async Task<IActionResult> GetProduct(int id)
        {
            var products = await epeyContext.Products
                      .Include(x => x.ProductCategoryNavigation)
                      .Include(p => p.Values)
                      .ThenInclude(v => v.Attribute)
                      .Where(p => p.ProductId == id).ToListAsync();
                      //.Where(p => CompareItems.CompareItemId.Contains(p.ProductId.ToString())).ToListAsync();


            var productDetails = products.Select(x => new
            {
                ProductName = x.ProductName,
                ProductId= x.ProductId,
                ProductImage = x.ProductImage,
                Values = x.Values.Select(v => new { v.Attribute.AttributeName, Value = GetValue(v) }).GroupBy(x => x.AttributeName)
            });

            return View(productDetails);
        }

        public IActionResult CreateProductCategory()
        {
            var productDetails = epeyContext.Categories.Select(c => new SelectListItem
            {

                Value = c.Id.ToString(),
                Text = c.Name,

            });

            ViewBag.ProductCategory = productDetails;


            return View();
        }

        public async Task<IActionResult> ListAttributeSpecs()
        {
            var specs = await epeyContext.AttributeSpecs.ToListAsync();
            return View(specs);
        }

        private int CompareValues(Value value1, Value value2)
        {
            switch (value1.Attribute.AttributeType)
            {
                case "integer":
                    int? int1 = value1.ValueInteger;
                    int? int2 = value2.ValueInteger;
                    return Nullable.Compare(int1, int2);

                case "float":
                    double? float1 = value1.ValueFloat;
                    double? float2 = value2.ValueFloat;
                    return Nullable.Compare(float1, float2);

                case "date":
                    DateOnly? date1 = value1.ValueDate;
                    DateOnly? date2 = value2.ValueDate;
                    return Nullable.Compare(date1, date2);

                case "string":
                    string string1 = value1.ValueString;
                    string string2 = value2.ValueString;
                    return string.Compare(string1, string2, StringComparison.Ordinal);

                case "boolean":
                    int? bool1 = value1.ValueBoolean;
                    int? bool2 = value2.ValueBoolean;
                    return Nullable.Compare(bool1, bool2);

                default:
                    return 0;
            }
        }

        public async Task<IActionResult> CompareProducts()
        {

            var productIds = CompareItems.CompareItemId.Select(id => int.Parse(id)).ToList();
            var products = await epeyContext.Products
                .Include(p => p.Values)
                .ThenInclude(v => v.Attribute)
                .ThenInclude(a => a.AttributeSpecNavigation)  // Ensure this is included
                .Where(p => productIds.Contains(p.ProductId))
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound();
            }

            var allAttributes = products.SelectMany(p => p.Values.Select(v => v.Attribute)).Distinct().ToList();

            var comparisonResults = new List<ComparisonResult>();

            foreach (var attribute in allAttributes)
            {
                var comparisonResult = new ComparisonResult
                {
                    AttributeName = attribute.AttributeName,
                    AttributeSpecName = attribute.AttributeSpecNavigation?.AttributeSpecsName,
                    ProductValues = new List<ProductAttributeValue>()
                };

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

                comparisonResults.Add(comparisonResult);
            }

            return View(comparisonResults);
        }


    
        public IActionResult CreateAttributeSpecs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttributeSpecs(AttributeSpec attributeSpec)
        {
            var test = await epeyContext.AttributeSpecs.AddAsync(new AttributeSpec
            {
                AttributeSpecsName = attributeSpec.AttributeSpecsName,
            });

            var test2 = await epeyContext.SaveChangesAsync();
            return RedirectToAction(nameof(ListAttributeSpecs));

        }


        public IActionResult CreateAttribute()
        {
            var category = epeyContext.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            var specs = epeyContext.AttributeSpecs.Select(s => new SelectListItem
            {
                Value = s.AttributeSpecsId.ToString(),
                Text = s.AttributeSpecsName
            });

            ViewBag.Category = category;
            ViewBag.Specs = specs;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAttribute(CreateAttribute_VM createAttribute_VM)
        {
            int attributeId = 0;
            var attirbuteModel = new Models.Attribute { AttributeName = createAttribute_VM.attribute.AttributeName, AttributeType = createAttribute_VM.attribute.AttributeType, AttributeSpec = createAttribute_VM.attributeSpec.AttributeSpecsId };

            var isAttributeExist = epeyContext.Attributes.FirstOrDefault(x => x.AttributeName == attirbuteModel.AttributeName && x.AttributeSpec == attirbuteModel.AttributeSpec && x.AttributeType == attirbuteModel.AttributeType);


            if (isAttributeExist != null)
            {
                attributeId = isAttributeExist.AttributeId;
            }
            else
            {
                var context = await epeyContext.Attributes.AddAsync(attirbuteModel);
                await epeyContext.SaveChangesAsync();
                attributeId = context.Entity.AttributeId;
            }

            var category = epeyContext.Categories.Find(createAttribute_VM.categories.Id);

            if (category != null && attirbuteModel != null)
            {
                epeyContext.CategoryAttributes.Add(new CategoryAttribute { AttributeId = attributeId, CategoryId = category.Id });
            }

            await epeyContext.SaveChangesAsync();

            return RedirectToAction(nameof(CreateAttribute));
        }

        public async Task<IActionResult> CreateProduct(int id)
        {
            var attributes = await epeyContext.Attributes.Include(x=>x.CategoryAttributes).Where(x => x.CategoryAttributes.Any(ca => ca.CategoryId == id)).ToListAsync();
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
            return View(model);
        }

        public IActionResult CreateProductList()
        {
            var categories = epeyContext.Categories.ToList();
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "photos");
            string filePath = Path.Combine(uploadPath, model.ProductImage.FileName);

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

                await model.ProductImage.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
            catch (Exception er)
            {
                throw er;
            }

            var product = new Product
            {
                ProductName = model.ProductName,
                ProductCategory = model.ProductCategory,
                ProductCreatedAt =  DateTime.Now.ToString(),
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

            epeyContext.Products.Add(product);
            await epeyContext.SaveChangesAsync();
            return RedirectToAction(nameof(CreateProduct), new { id = model.ProductCategory });
        }



    }
}
