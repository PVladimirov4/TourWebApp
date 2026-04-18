using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore; // ЗАДЪЛЖИТЕЛНО за Include
using TourWebApp.Core.Contracts;
using TourWebApp.Infrastructure.Data;
using TourWebApp.Infrastructure.Data.Entities;

namespace TourWebApp.Core.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly ApplicationDbContext _context;

        public HolidayService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Country> GetCountrys()
        {
            return _context.Countrys.ToList();
        }

        public bool Create(string name, int countryId, int categoryId, string picture, int quantity, decimal price, decimal discount, string description, DateTime departureTime, DateTime arrivalDate)
        {
            Holiday item = new Holiday
            {
                HolidayName = name,
                CountryId = countryId,
                CategoryId = categoryId,
                Picture = picture,
                Quantity = quantity,
                Price = price,
                Discount = discount,
                Description = description,
                DepartureTime = departureTime,
                ArrivalDate = arrivalDate
            };

            _context.Holidays.Add(item);
            return _context.SaveChanges() > 0;
        }

        public Holiday GetHolidayById(int holidayId)
        {
            // Използваме Include, за да не са null държавата и категорията в Details
            return _context.Holidays
                .Include(h => h.Country)
                .Include(h => h.Category)
                .FirstOrDefault(h => h.Id == holidayId);
        }

        public List<Holiday> GetHolidays()
        {
            return _context.Holidays
                .Include(h => h.Country)
                .Include(h => h.Category)
                .ToList();
        }

        public List<Holiday> GetHolidays(string searchStringCategoryName, string searchStringcountryName)
        {
            // Правим заявката към базата с включени обекти
            var query = _context.Holidays
                .Include(h => h.Country)
                .Include(h => h.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchStringCategoryName))
            {
                query = query.Where(x => x.Category.CategoryName.ToLower().Contains(searchStringCategoryName.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchStringcountryName))
            {
                query = query.Where(x => x.Country.CountryName.ToLower().Contains(searchStringcountryName.ToLower()));
            }

            return query.ToList();
        }

        public bool RemoveById(int holidayId)
        {
            var holiday = _context.Holidays.Find(holidayId);
            if (holiday == null) return false;

            _context.Holidays.Remove(holiday);
            return _context.SaveChanges() > 0;
        }

        public bool Update(int holidayId, string name, int countryId, int categoryId, string picture, int quantity, decimal price, decimal discount, string description, DateTime departureTime, DateTime arrivalDate)
        {
            var holiday = _context.Holidays.Find(holidayId);
            if (holiday == null) return false;

            holiday.HolidayName = name;
            holiday.CountryId = countryId; // Директно обновяваме външния ключ
            holiday.CategoryId = categoryId; // Директно обновяваме външния ключ
            holiday.Picture = picture;
            holiday.Quantity = quantity;
            holiday.Price = price;
            holiday.Discount = discount;
            holiday.Description = description;
            holiday.DepartureTime = departureTime;
            holiday.ArrivalDate = arrivalDate;

            _context.Holidays.Update(holiday);
            return _context.SaveChanges() > 0;
        }

        public List<int> GetFavoriteIds(string userId)
        {
            return _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.HolidayId)
                .ToList();
        }
    }
}