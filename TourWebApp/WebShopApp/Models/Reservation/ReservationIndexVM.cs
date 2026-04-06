namespace TourWebApp.Models.Reservation
{
    public class ReservationIndexVM
    {
        public int Id { get; set; }
        public string ReservationDate { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string User { get; set; } = null!;

        public int HolidayId { get; set; }
        public string Holiday { get; set; } = null!;
        public string Picture { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

