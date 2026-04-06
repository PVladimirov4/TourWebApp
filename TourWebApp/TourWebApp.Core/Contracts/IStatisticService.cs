using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourWebApp.Core.Contracts
{
    public interface IStatisticService
    {
        int CountHolidays();
        int CountClients();
        int CountReservations();
        decimal SumReservations();
    }
}
