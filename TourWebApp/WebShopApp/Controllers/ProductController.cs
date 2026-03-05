using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TourWebApp.Core.Contracts;
using TourWebApp.Infrastructure.Data.Entities;
using TourWebApp.Models.country;
using TourWebApp.Models.Category;
using TourWebApp.Models.Product;
using TourWebApp.Core.Services;

namespace TourWebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICountryService _countryService;

        public ProductController(IProductService productService, ICategoryService categoryService, ICountryService countryService)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._countryService = countryService;
        }

        // GET: ProductController
        [AllowAnonymous]
        public ActionResult Index(string searchStringCategoryName, string searchStringCountryName)
        {
            List<ProductIndexVM> products = _productService.GetProducts(searchStringCategoryName, searchStringCountryName)
     .Select(product => new ProductIndexVM
     {
         Id = product.Id,
         ProductName = product.ProductName,
         CountryId = product.CountryId,
         CountryName = product.Country.CountryName,
         CategoryId = product.CategoryId,
         CategoryName = product.Category.CategoryName,
         Picture = product.Picture,
         Quantity = product.Quantity,
         Price = product.Price,
         Discount = product.Discount,
         Description = product.Description
     })
     .ToList();

            return this.View(products);
        }

        // GET: ProductController/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var item = _productService.GetProductById(id);

            if (item == null)
            {
                return NotFound();
            }

            var product = new ProductDetailsVM
            {
                Id = item.Id,
                ProductName = item.ProductName,
                CountryId = item.CountryId,
                CountryName = item.Country.CountryName,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.CategoryName,
                Picture = item.Picture,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount,
                Description = item.Description
            };

            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            var product = new ProductCreateVM();

            product.Countrys = _countryService.GetCountrys()
                .Select(x => new CountryPairVM()
                {
                    Id = x.Id,
                    Name = x.CountryName
                }).ToList();

            product.Categories = _categoryService.GetCategories()
                .Select(x => new CategoryPairVM()
                {
                    Id = x.Id,
                    Name = x.CategoryName
                }).ToList();

            return View(product);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] ProductCreateVM product)
        {
            if (ModelState.IsValid)
            {
                var createdId = _productService.Create(product.ProductName, product.CountryId, product.CategoryId, product.Picture, product.Quantity, product.Price, product.Discount, product.Description);

                if (createdId)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            ProductEditVM updatedProduct = new ProductEditVM()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CountryId = product.CountryId,
                //countryName = product.country.countryName,
                CategoryId = product.CategoryId,
                // CategoryName = product.Category.CategoryName,
                Picture = product.Picture,
                Quantity = product.Quantity,
                Price = product.Price,
                Discount = product.Discount
            };
            updatedProduct.Countrys = _countryService.GetCountrys()
                .Select(b => new CountryPairVM()
                {
                 Id = b.Id,
                Name = b.CountryName
                })
                  .ToList();

            updatedProduct.Categories = _categoryService.GetCategories()
                .Select(c => new CategoryPairVM()
                {
                    Id = c.Id,
                    Name = c.CategoryName
                })
                .ToList();

            return View(updatedProduct);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductEditVM product)
        {
            if (ModelState.IsValid)
            {
                var updated = _productService.Update(id, product.ProductName, product.CountryId,
                    product.CategoryId, product.Picture,
                    product.Quantity, product.Price, product.Discount, product.Description);

                if (updated)
                {
                    return this.RedirectToAction("Index");
                }
            }

            return View(product);
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            Product item = _productService.GetProductById(id);

            if (item == null)
            {
                return NotFound();
            }

            ProductDeleteVM product = new ProductDeleteVM()
            {
                Id = item.Id,
                ProductName = item.ProductName,
                CountryId = item.CountryId,
                CountryName = item.Country.CountryName,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.CategoryName,
                Picture = item.Picture,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount,
                Description = item.Description
            };

            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
           var deleted = _productService.RemoveById(id);

            if (deleted)
            {
                return this.RedirectToAction("Success");
            }
            else
            {
                return View();
            }

        }
        public IActionResult Success()
        {
            return View();
        }
    }
}
