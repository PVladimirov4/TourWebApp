using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourWebApp.Infrastructure.Data.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public int HolidayId { get; set; }
        public virtual Holiday Holiday { get; set; } = null!;

        [Required] 
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get { return this.Quantity * this.Price - this.Quantity * this.Price * this.Discount / 100; } }
    }
}
