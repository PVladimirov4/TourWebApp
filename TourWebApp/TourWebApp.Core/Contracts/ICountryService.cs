using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TourWebApp.Infrastructure.Data.Entities;

namespace TourWebApp.Core.Contracts
{
    public interface ICountryService
    {
        List<Country> GetCountrys();
        Country GetCountryById(int countryId);
        List<Country> GetHolidaysByCountry(int countryId);
    }
}
