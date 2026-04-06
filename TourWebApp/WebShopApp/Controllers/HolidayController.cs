using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourWebApp.Core.Contracts;
using TourWebApp.Infrastructure.Data.Entities;
using TourWebApp.Models.country;
using TourWebApp.Models.Category;
using TourWebApp.Models.Holiday;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TourWebApp.Infrastructure.Data;

namespace TourWebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class HolidayController : Controller
    {
        private readonly IHolidayService _holidayService;
        private readonly ICategoryService _categoryService;
        private readonly ICountryService _countryService;
        private readonly ApplicationDbContext _context;

        public HolidayController(IHolidayService holidayService, ICategoryService categoryService, ICountryService countryService, ApplicationDbContext context)
        {
            this._holidayService = holidayService;
            this._categoryService = categoryService;
            this._countryService = countryService;
            _context = context;
        }

        // GET: HolidayController
        [AllowAnonymous]
        public ActionResult Index(string searchStringCategoryName, int? countryId, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            // 1. Вземаме първоначалния списък от сървиса
            var filteredHolidays = _holidayService.GetHolidays(searchStringCategoryName, null);

            // 2. Филтър по Държава
            if (countryId.HasValue && countryId.Value > 0)
            {
                filteredHolidays = filteredHolidays.Where(h => h.CountryId == countryId.Value).ToList();
            }

            // 3. Филтър по Категория
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                filteredHolidays = filteredHolidays.Where(h => h.CategoryId == categoryId.Value).ToList();
            }

            // --- НОВО: Филтър по цена ---
            if (minPrice.HasValue)
            {
                filteredHolidays = filteredHolidays.Where(h => h.Price >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                filteredHolidays = filteredHolidays.Where(h => h.Price <= maxPrice.Value).ToList();
            }

            // 4. Мапване към ViewModel
            List<HolidayIndexVM> holidays = filteredHolidays
                .Select(holiday => new HolidayIndexVM
                {
                    Id = holiday.Id,
                    HolidayName = holiday.HolidayName,
                    CountryId = holiday.CountryId,
                    CountryName = holiday.Country?.CountryName ?? "Unknown",
                    CategoryId = holiday.CategoryId,
                    CategoryName = holiday.Category?.CategoryName ?? "General",
                    Picture = holiday.Picture,
                    Quantity = holiday.Quantity,
                    Price = holiday.Price,
                    Discount = holiday.Discount,
                    Description = holiday.Description,
                    DepartureTime = holiday.DepartureTime,
                    ArrivalDate = holiday.ArrivalDate,
                })
                .ToList();

            // Данни за падащите менюта (Dropdowns)
            ViewBag.Countries = _countryService.GetCountrys()
                .Select(c => new CountryPairVM { Id = c.Id, Name = c.CountryName }).ToList();

            ViewBag.Categories = _categoryService.GetCategories()
                .Select(c => new CategoryPairVM { Id = c.Id, Name = c.CategoryName }).ToList();

            // Запазваме избраните стойности, за да ги покажем във формата
            ViewBag.SelectedCountry = countryId;
            ViewBag.SelectedCategory = categoryId;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            // Логика за любими
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.FavoriteHolidayIds = _context.Favorites
                    .Where(f => f.UserId == userId)
                    .Select(f => f.HolidayId)
                    .ToList();
            }
            else
            {
                ViewBag.FavoriteHolidayIds = new List<int>();
            }

            return View(holidays);
        }

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var item = _holidayService.GetHolidayById(id);

            if (item == null) return NotFound();

            var holiday = new HolidayDetailsVM
            {
                Id = item.Id,
                HolidayName = item.HolidayName,
                CountryId = item.CountryId,
                CountryName = item.Country?.CountryName,
                CategoryId = item.CategoryId,
                CategoryName = item.Category?.CategoryName,
                Picture = item.Picture,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount,
                Description = item.Description,
                DepartureTime = item.DepartureTime,
                ArrivalDate = item.ArrivalDate
            };

            return View(holiday);
        }

        public ActionResult Create()
        {
            var holiday = new HolidayCreateVM();
            holiday.Countrys = _countryService.GetCountrys().Select(x => new CountryPairVM { Id = x.Id, Name = x.CountryName }).ToList();
            holiday.Categories = _categoryService.GetCategories().Select(x => new CategoryPairVM { Id = x.Id, Name = x.CategoryName }).ToList();
            return View(holiday);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] HolidayCreateVM holiday)
        {
            if (ModelState.IsValid)
            {
                var created = _holidayService.Create(holiday.HolidayName, holiday.CountryId, holiday.CategoryId, holiday.Picture, holiday.Quantity, holiday.Price, holiday.Discount, holiday.Description, holiday.DepartureTime, holiday.ArrivalDate);
                if (created) return RedirectToAction(nameof(Index));
            }
            return View(holiday);
        }

        public ActionResult Edit(int id)
        {
            Holiday holiday = _holidayService.GetHolidayById(id);
            if (holiday == null) return NotFound();

            HolidayEditVM updatedHoliday = new HolidayEditVM()
            {
                Id = holiday.Id,
                HolidayName = holiday.HolidayName,
                CountryId = holiday.CountryId,
                CategoryId = holiday.CategoryId,
                Description = holiday.Description,
                Picture = holiday.Picture,
                Quantity = holiday.Quantity,
                Price = holiday.Price,
                Discount = holiday.Discount,
                DepartureTime = holiday.DepartureTime,
                ArrivalDate = holiday.ArrivalDate
            };

            updatedHoliday.Countrys = _countryService.GetCountrys().Select(b => new CountryPairVM { Id = b.Id, Name = b.CountryName }).ToList();
            updatedHoliday.Categories = _categoryService.GetCategories().Select(c => new CategoryPairVM { Id = c.Id, Name = c.CategoryName }).ToList();

            return View(updatedHoliday);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HolidayEditVM holiday)
        {
            if (ModelState.IsValid)
            {
                var updated = _holidayService.Update(id, holiday.HolidayName, holiday.CountryId, holiday.CategoryId, holiday.Picture, holiday.Quantity, holiday.Price, holiday.Discount, holiday.Description, holiday.DepartureTime, holiday.ArrivalDate);
                if (updated) return RedirectToAction("Index");
            }
            return View(holiday);
        }

        public ActionResult Delete(int id)
        {
            Holiday item = _holidayService.GetHolidayById(id);
            if (item == null) return NotFound();

            var holiday = new HolidayDeleteVM()
            {
                Id = item.Id,
                HolidayName = item.HolidayName,
                CountryName = item.Country?.CountryName,
                CategoryName = item.Category?.CategoryName,
                Picture = item.Picture,
                Price = item.Price
            };

            return View(holiday);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var deleted = _holidayService.RemoveById(id);
            return deleted ? RedirectToAction("Success") : View();
        }

        public IActionResult Success() => View();
    }
}