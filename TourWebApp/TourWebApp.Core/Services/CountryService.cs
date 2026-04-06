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

            public List<Holiday> GetHolidaysByCountry(int countryId)
            {
                return _context.Holidays
                    .Where(x => x.CountryId == countryId)
                    .ToList();
            }

        List<Country> ICountryService.GetHolidaysByCountry(int countryId)
        {
            throw new NotImplementedException();
        }
    }
}
