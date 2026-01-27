using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourWebApp.Infrastructure.Data.Entities
{
    public class Product
    {
        [Required]
        [MaxLength(30)]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; } = null!;

        [Required]
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; } = null!;

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;

        public string Picture { get; set; } = null!;

        public string Description { get; set; } = null!;

        [Range(0, 5000)]
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; } = new List<Order>();
    }
}
