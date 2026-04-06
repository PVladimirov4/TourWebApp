using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Globalization;
using System.Security.Claims;

using TourWebApp.Core.Contracts;
using TourWebApp.Core.Services;
using TourWebApp.Infrastructure.Data.Entities;
using TourWebApp.Models.Reservation;

namespace TourWebApp.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly IHolidayService _holidayService;
        private readonly IReservationService _reservationService;

        public ReservationController(IHolidayService holidayService, IReservationService reservationService)
        {
            _holidayService = holidayService;
            _reservationService = reservationService;
        }

        // GET: ReservationController
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            List<ReservationIndexVM> reservations = _reservationService.GetReservations()
     .Select(x => new ReservationIndexVM
     {
         Id = x.Id,
         ReservationDate = x.ReservationDate.ToString("dd-MMM-yyyy hh:mm", CultureInfo.InvariantCulture),
         UserId = x.UserId,
         User = x.User.UserName,
         HolidayId = x.HolidayId,
         Holiday = x.Holiday.HolidayName,     
         Picture = x.Holiday.Picture,
         Quantity = x.Quantity,
         Price = x.Price,
         Discount = x.Discount,
         TotalPrice = x.TotalPrice
     })
     .ToList();

            return View(reservations);
        }

        // GET: ReservationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReservationController/Create
        public ActionResult Create(int id)
        {
            Holiday holiday = _holidayService.GetHolidayById(id);

            if (holiday == null)
            {
                return NotFound();
            }

            ReservationCreateVM reservation = new ReservationCreateVM()
            {
                HolidayId = holiday.Id,
                HolidayName = holiday.HolidayName,
                QuantityInStock = holiday.Quantity,
                Price = holiday.Price,
                Discount = holiday.Discount,
                Picture = holiday.Picture
            };

            return View(reservation);
        }

        // POST: ReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservationCreateVM bindingModel)
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var holiday = this._holidayService.GetHolidayById(bindingModel.HolidayId);

            if (currentUserId == null || holiday == null || holiday.Quantity < bindingModel.Quantity || holiday.Quantity == 0)
            {
                
                ModelState.AddModelError("", "Reservation denied");
                return RedirectToAction("Denied", "Reservation");
            }

            _reservationService.Create(bindingModel.HolidayId, currentUserId, bindingModel.Quantity);
           
            return this.RedirectToAction("Index", "Holiday");
        }

        // GET: ReservationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReservationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReservationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Denied()
        {
            return View();
        }
        public ActionResult MyReservations()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(); // or RedirectToAction("Login", "Account")
            }

            var reservations = _reservationService.GetReservationsByUser(currentUserId)
                .Select(x => new ReservationIndexVM
                {
                    Id = x.Id,
                    ReservationDate = x.ReservationDate.ToString("dd-MMM-yyyy hh:mm", CultureInfo.InvariantCulture),
                    UserId = x.UserId,
                    User = x.User.UserName,
                    HolidayId = x.HolidayId,
                    Holiday = x.Holiday.HolidayName,
                    Picture = x.Holiday.Picture,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    Discount = x.Discount,
                    TotalPrice = x.TotalPrice
                })
                .ToList();

            return View(reservations);
        }
    }
}
