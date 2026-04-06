using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TourWebApp.Core.Contracts;
using TourWebApp.Infrastructure.Data;
using TourWebApp.Infrastructure.Data.Entities;

namespace TourWebApp.Core.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHolidayService _holidayService;

        public ReservationService(ApplicationDbContext context, IHolidayService holidayService)
        {
            _context = context;
            _holidayService = holidayService;
        }
        public bool Create(int holidayId, string userId, int quantity) 
        {
            var holiday = this._context.Holidays.SingleOrDefault(x => x.Id == holidayId);

            if (holiday == null)
            {
                return false;
            }
            Reservation item = new Reservation
            {
                ReservationDate = DateTime.Now,
                HolidayId = holidayId,
                UserId = userId,
                Quantity = quantity,
                Price = holiday.Price,
                Discount = holiday.Discount
            };

            holiday.Quantity -= quantity;

            this._context.Holidays.Update(holiday);
            this._context.Reservations.Add(item);

            return _context.SaveChanges() != 0;
        }

        public Reservation GetReservationById(int reservationId)
        {
            throw new NotImplementedException();
        }

        public List<Reservation> GetReservations()
        {
            return _context.Reservations.OrderByDescending(x=>x.ReservationDate).ToList();
        }
        public List<Reservation> GetReservationsByUser(string userId) 
        {
            return _context.Reservations.Where(x=>x.UserId == userId).OrderByDescending(x => x.ReservationDate).ToList();
        }

        public bool RemoveById(int reservationId)
        {
            throw new NotImplementedException();
        }

        public bool Update(int reservationId, int holidayId, string userId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
