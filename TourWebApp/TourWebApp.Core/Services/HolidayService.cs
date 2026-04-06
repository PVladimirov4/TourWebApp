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
    public class HolidayService : IHolidayService
    {

        private readonly ApplicationDbContext _context;
        public List<Country> GetCountrys()
        {
            return _context.Countrys.ToList(); // Увери се, че името на таблицата е точно такова
        }

        public HolidayService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(string name, int countryId, int categoryId, string picture, int quantity, decimal price, decimal discount, string description, DateTime departureTime, DateTime arrivalDate)
        {
            Holiday item = new Holiday
            {
                HolidayName = name,
                Country = _context.Countrys.Find(countryId),
                Category = _context.Categories.Find(categoryId),
                Picture = picture,
                Quantity = quantity,
                Price = price,
                Discount = discount,
                Description = description,
                DepartureTime = departureTime,
                ArrivalDate = arrivalDate
            };

            _context.Holidays.Add(item);
            return _context.SaveChanges() != 0;
        }

        public Holiday GetHolidayById(int holidayId)
        {
            return _context.Holidays.Find(holidayId);
        }

        public List<Holiday> GetHolidays()
        {
            List<Holiday> holidays = _context.Holidays.ToList();
            return holidays;
        }

        public List<Holiday> GetHolidays(string searchStringCategoryName, string searchStringcountryName)
        {
            List<Holiday> holidays = _context.Holidays.ToList();
            if (!String.IsNullOrEmpty(searchStringCategoryName) && !String.IsNullOrEmpty(searchStringcountryName))
            {
                holidays = holidays.Where(x =>
              x.Category.CategoryName.ToLower().Contains   (searchStringCategoryName.ToLower())
               && x.Country.CountryName.ToLower().Contains      (searchStringcountryName.ToLower())
               ).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringCategoryName))
            {
                holidays = holidays.Where(x => x.Category.CategoryName.ToLower ().Contains (searchStringCategoryName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringcountryName))
            {
                holidays = holidays.Where(x => x.Country.CountryName.ToLower().Contains(searchStringcountryName.ToLower())).ToList();
            } // ToList();
            return holidays;
        }
        
        public bool RemoveById(int holidayId)
        {
            var holiday = GetHolidayById(holidayId);
            if(holiday == default(Holiday)) return false;
            _context.Remove(holiday);
            return _context.SaveChanges() != 0;
        }

        public bool Update(int holidayId, string name, int countryId, int categoryId, string picture, int quantity, decimal price, decimal discount, string description, DateTime departureTime, DateTime arrivalDate)
        {
            var holiday = GetHolidayById(holidayId);
            if (holiday == default(Holiday))
            {
                return false;
            }

            holiday.HolidayName = name;

            //holiday.countryId = countryId;
            //holiday.CategoryId = categoryId;

            holiday.Country = _context.Countrys.Find(countryId);
            holiday.Category = _context.Categories.Find(categoryId);

            holiday.Picture = picture;
            holiday.Quantity = quantity;
            holiday.Price = price;
            holiday.Discount = discount;
            holiday.Description = description;
            holiday.DepartureTime = departureTime;
            holiday.ArrivalDate = arrivalDate;

            _context.Update(holiday);
            return _context.SaveChanges() != 0;

        }
    }
}
