using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TourWebApp.Core.Contracts;
using TourWebApp.Infrastructure.Data;

namespace TourWebApp.Core.Services
{
    public  class StatisticService : IStatisticService
    {
        private readonly ApplicationDbContext _context;

        public StatisticService(ApplicationDbContext context)
        {
            _context = context;
        }
        public int CountClients()
        {
            return _context.Reservations.Count();
        }
        public int CountReservations()
        {
            return _context.Reservations.Count();
        }
        public int CountHolidays()
        {
            return _context.Holidays.Count();
        }
        public decimal SumReservations()
        {
            var suma = _context.Reservations.Sum(x => x.TotalPrice);
            return suma;
        }
    }
   
}
