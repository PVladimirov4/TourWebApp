using System.ComponentModel.DataAnnotations;

namespace TourWebApp.Models.Statistic
{
    public class StatisticVM
    {
        [Display(Name = "Count Clients")]
        public int CountClients { get; set; }
        [Display(Name = "Count Holidays")]
        public int CountHolidays { get; set; }
        [Display(Name = "Count Reservations")]
        public int CountReservations { get; set; }
        [Display(Name = "Total Sum Reservations ")]
        public decimal SumReservations { get; set; }

    }
}
