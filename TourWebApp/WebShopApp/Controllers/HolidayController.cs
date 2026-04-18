using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourWebApp.Core.Contracts;
using TourWebApp.Models.country;
using TourWebApp.Models.Category;
using TourWebApp.Models.Holiday;
using System.Security.Claims;

namespace TourWebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class HolidayController : Controller
    {
        private readonly IHolidayService _holidayService;
        private readonly ICategoryService _categoryService;
        private readonly ICountryService _countryService;

        public HolidayController(IHolidayService holidayService, ICategoryService categoryService, ICountryService countryService)
        {
            _holidayService = holidayService;
            _categoryService = categoryService;
            _countryService = countryService;
        }

        [AllowAnonymous]
        public ActionResult Index(string searchStringCategoryName, int? countryId, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var filteredHolidays = _holidayService.GetHolidays(searchStringCategoryName, null);

            // Филтриране (остава както беше, но без _context)
            if (countryId.HasValue && countryId.Value > 0)
                filteredHolidays = filteredHolidays.Where(h => h.CountryId == countryId.Value).ToList();

            if (categoryId.HasValue && categoryId.Value > 0)
                filteredHolidays = filteredHolidays.Where(h => h.CategoryId == categoryId.Value).ToList();

            if (minPrice.HasValue)
                filteredHolidays = filteredHolidays.Where(h => h.Price >= minPrice.Value).ToList();

            if (maxPrice.HasValue)
                filteredHolidays = filteredHolidays.Where(h => h.Price <= maxPrice.Value).ToList();

            var holidays = filteredHolidays.Select(h => new HolidayIndexVM
            {
                Id = h.Id,
                HolidayName = h.HolidayName,
                CountryName = h.Country?.CountryName ?? "Unknown",
                CategoryName = h.Category?.CategoryName ?? "General",
                Picture = h.Picture,
                Price = h.Price,
                DepartureTime = h.DepartureTime,
                ArrivalDate = h.ArrivalDate
            }).ToList();

            ViewBag.Countries = _countryService.GetCountrys().Select(c => new CountryPairVM { Id = c.Id, Name = c.CountryName }).ToList();
            ViewBag.Categories = _categoryService.GetCategories().Select(c => new CategoryPairVM { Id = c.Id, Name = c.CategoryName }).ToList();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.FavoriteHolidayIds = _holidayService.GetFavoriteIds(userId);
            }
            ViewBag.CurrentSearch = searchStringCategoryName;
            ViewBag.CurrentCountry = countryId;
            ViewBag.CurrentCategory = categoryId;
            ViewBag.CurrentMinPrice = minPrice;
            ViewBag.CurrentMaxPrice = maxPrice;
            return View(holidays);
        }

        public ActionResult Create()
        {
            var holiday = new HolidayCreateVM();
            holiday.DepartureTime = DateTime.Now;
            holiday.ArrivalDate = DateTime.Now.AddDays(7);

            // Зареждане на списъците
            holiday.Countrys = _countryService.GetCountrys().Select(x => new CountryPairVM { Id = x.Id, Name = x.CountryName }).ToList();
            holiday.Categories = _categoryService.GetCategories().Select(x => new CategoryPairVM { Id = x.Id, Name = x.CategoryName }).ToList();

            return View(holiday);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HolidayCreateVM holiday)
        {
            if (ModelState.IsValid)
            {
                var created = _holidayService.Create(
                    holiday.HolidayName, holiday.CountryId, holiday.CategoryId,
                    holiday.Picture, holiday.Quantity, holiday.Price,
                    holiday.Discount, holiday.Description, holiday.DepartureTime, holiday.ArrivalDate);

                if (created) return RedirectToAction(nameof(Index));
            }

            // Ако има грешка, ТРЯБВА пак да заредим списъците, иначе страницата ще гръмне
            holiday.Countrys = _countryService.GetCountrys().Select(x => new CountryPairVM { Id = x.Id, Name = x.CountryName }).ToList();
            holiday.Categories = _categoryService.GetCategories().Select(x => new CategoryPairVM { Id = x.Id, Name = x.CategoryName }).ToList();

            return View(holiday);
        }
        [AllowAnonymous] // Всеки може да гледа детайли
        public ActionResult Details(int id)
        {
            var holiday = _holidayService.GetHolidayById(id);

            if (holiday == null)
            {
                return NotFound();
            }

            // Прехвърляме данните от Entity към ViewModel
            var model = new HolidayDetailsVM
            {
                Id = holiday.Id,
                HolidayName = holiday.HolidayName,
                CountryName = holiday.Country?.CountryName ?? "Unknown",
                CategoryName = holiday.Category?.CategoryName ?? "General",
                Picture = holiday.Picture,
                Price = holiday.Price,
                Discount = holiday.Discount,
                Description = holiday.Description,
                DepartureTime = holiday.DepartureTime,
                ArrivalDate = holiday.ArrivalDate,
                Quantity = holiday.Quantity
            };

            return View(model);
        }
        // GET: Holiday/Edit/5
        public ActionResult Edit(int id)
        {
            var holiday = _holidayService.GetHolidayById(id);
            if (holiday == null) return NotFound();

            var model = new HolidayEditVM
            {
                Id = holiday.Id,
                HolidayName = holiday.HolidayName,
                CountryId = holiday.CountryId,
                CategoryId = holiday.CategoryId,
                Picture = holiday.Picture,
                Price = holiday.Price,
                Discount = holiday.Discount,
                Description = holiday.Description,
                Quantity = holiday.Quantity,
                DepartureTime = holiday.DepartureTime,
                ArrivalDate = holiday.ArrivalDate,
                // Зареждаме списъците за падащите менюта
                Countrys = _countryService.GetCountrys().Select(x => new CountryPairVM { Id = x.Id, Name = x.CountryName }).ToList(),
                Categories = _categoryService.GetCategories().Select(x => new CategoryPairVM { Id = x.Id, Name = x.CategoryName }).ToList()
            };

            return View(model);
        }

        // POST: Holiday/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HolidayEditVM model)
        {
            if (ModelState.IsValid)
            {
                var updated = _holidayService.Update(
                    model.Id, model.HolidayName, model.CountryId, model.CategoryId,
                    model.Picture, model.Quantity, model.Price, model.Discount,
                    model.Description, model.DepartureTime, model.ArrivalDate);

                if (updated) return RedirectToAction(nameof(Index));
            }

            // Ако има грешка, презареждаме списъците
            model.Countrys = _countryService.GetCountrys().Select(x => new CountryPairVM { Id = x.Id, Name = x.CountryName }).ToList();
            model.Categories = _categoryService.GetCategories().Select(x => new CategoryPairVM { Id = x.Id, Name = x.CategoryName }).ToList();
            return View(model);
        }
        // GET: Holiday/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var holiday = _holidayService.GetHolidayById(id);
            if (holiday == null)
            {
                return NotFound();
            }

            // ПРОМЯНА: Използваме HolidayDeleteVM вместо HolidayDetailsVM
            var model = new TourWebApp.Models.Holiday.HolidayDeleteVM
            {
                Id = holiday.Id,
                HolidayName = holiday.HolidayName,
                CountryName = holiday.Country?.CountryName ?? "Unknown",
                CategoryName = holiday.Category?.CategoryName ?? "General",
                Picture = holiday.Picture,
                Price = holiday.Price,
                
            };

            return View(model);
        }

        // POST: Holiday/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            var deleted = _holidayService.RemoveById(id);
            if (deleted)
            {
                return RedirectToAction(nameof(Index));
            }

            // Ако не успее да изтрие (например заради релации в базата), 
            // по-добре ни върни в списъка или покажи грешка
            return RedirectToAction(nameof(Index));
        }

        // Останалите методи Edit, Delete трябва да се изчистят по същия начин от _context
    }
}