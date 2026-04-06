using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TourWebApp.Infrastructure.Data.Entities;

namespace TourWebApp.Core.Contracts
{
    public interface IReservationService
    {
        bool Create(int holidayId, string userId, int quantity);

        List<Reservation> GetReservations();

        List<Reservation> GetReservationsByUser(string userId);

        Reservation GetReservationById(int reservationId);

        bool RemoveById(int reservationId);

        bool Update(int reservationId, int holidayId, string userId, int quantity);

    }
}
