using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TourWebApp.Core.Contracts;
using TourWebApp.Infrastructure.Data;
using TourWebApp.Infrastructure.Data.Entities;

namespace TourWebApp.Core.Services
{
    public class ProductService : IProductService
    {

        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(string name, int countryId, int categoryId, string picture, int quantity, decimal price, decimal discount, string description)
        {
            Product item = new Product
            {
                ProductName = name,
                Country = _context.Countrys.Find(countryId),
                Category = _context.Categories.Find(categoryId),
                Picture = picture,
                Quantity = quantity,
                Price = price,
                Discount = discount,
                Description = description
            };

            _context.Products.Add(item);
            return _context.SaveChanges() != 0;
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public List<Product> GetProducts()
        {
            List<Product> products = _context.Products.ToList();
            return products;
        }

        public List<Product> GetProducts(string searchStringCategoryName, string searchStringcountryName)
        {
            List<Product> products = _context.Products.ToList();
            if (!String.IsNullOrEmpty(searchStringCategoryName) && !String.IsNullOrEmpty(searchStringcountryName))
            {
                products = products.Where(x =>
              x.Category.CategoryName.ToLower().Contains   (searchStringCategoryName.ToLower())
               && x.Country.CountryName.ToLower().Contains      (searchStringcountryName.ToLower())
               ).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringCategoryName))
            {
               products = products.Where(x => x.Category.CategoryName.ToLower ().Contains (searchStringCategoryName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringcountryName))
            {
                products = products.Where(x => x.Country.CountryName.ToLower().Contains(searchStringcountryName.ToLower())).ToList();
            } // ToList();
            return products;
        }
        
        public bool RemoveById(int productId)
        {
            var product = GetProductById(productId);
            if(product == default(Product)) return false;
            _context.Remove(product);
            return _context.SaveChanges() != 0;
        }

        public bool Update(int productId, string name, int countryId, int categoryId, string picture, int quantity, decimal price, decimal discount, string description)
        {
            var product = GetProductById(productId);
            if (product == default(Product))
            {
                return false;
            }

            product.ProductName = name;

            //product.countryId = countryId;
            //product.CategoryId = categoryId;

            product.Country = _context.Countrys.Find(countryId);
            product.Category = _context.Categories.Find(categoryId);

            product.Picture = picture;
            product.Quantity = quantity;
            product.Price = price;
            product.Discount = discount;
            product.Description = description;

            _context.Update(product);
            return _context.SaveChanges() != 0;

        }
    }
}
