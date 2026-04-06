using System.ComponentModel.DataAnnotations;

namespace TourWebApp.Models.Reservation
{
    public class ReservationCreateVM
    {
        public int Id { get; set; }

        public DateTime ReservationDate { get; set; }

        public int HolidayId { get; set; }
        public string HolidayName { get; set; } = null!;
        public int QuantityInStock { get; set; }
        public string Picture { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
