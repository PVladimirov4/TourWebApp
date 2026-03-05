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
    public class CountryService : ICountryService
    {
        private readonly ApplicationDbContext _context;                 

            public CountryService(ApplicationDbContext context)
            {
                _context = context;
            }

            public Country GetCountryById(int countryId)
            {
                return _context.Countrys.Find(countryId);
            }

            public List<Country> GetCountrys()
            {
                return _context.Countrys.ToList();
            }

            public List<Product> GetProductsByCountry(int countryId)
            {
                return _context.Products
                    .Where(x => x.CountryId == countryId)
                    .ToList();
            }

        List<Country> ICountryService.GetProductsByCountry(int countryId)
        {
            throw new NotImplementedException();
        }
    }
}
