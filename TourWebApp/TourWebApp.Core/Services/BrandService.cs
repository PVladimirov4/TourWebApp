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
    public class BrandService : IBrandService
    {
        private readonly ApplicationDbContext _context;                 

            public BrandService(ApplicationDbContext context)
            {
                _context = context;
            }

            public Brand GetBrandById(int brandId)
            {
                return _context.Brands.Find(brandId);
            }

            public List<Brand> GetBrands()
            {
                return _context.Brands.ToList();
            }

            public List<Product> GetProductsByBrand(int brandId)
            {
                return _context.Products
                    .Where(x => x.BrandId == brandId)
                    .ToList();
            }       
    }
}
