using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TourWebApp.Infrastructure.Data.Entities;

namespace TourWebApp.Core.Contracts
{
    public interface IHolidayService
    {
        bool Create(string name, int countryId, int categoryId, string picture, int quantity, decimal price, decimal discount, string description, DateTime departureTime, DateTime arrivalDate);

        bool Update(int holidayId, string name, int countryId, int categoryId, string picture, int quantity, decimal price, decimal discount, string description, DateTime departureTime, DateTime arrivalDate);

        List<Holiday> GetHolidays();

        Holiday GetHolidayById(int holidayId);

        bool RemoveById(int dogholidayId);

        List<Holiday> GetHolidays(string searchStringCategoryName, string searchStringCountryName);

        List<Country> GetCountrys();
    }
}
