using System.ComponentModel.DataAnnotations;

using TourWebApp.Models.country;
using TourWebApp.Models.Category;

namespace TourWebApp.Models.Product
{
    public class ProductEditVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = null!;

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public virtual List<CountryPairVM> Countrys { get; set; } = new List<CountryPairVM>();

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public virtual List<CategoryPairVM> Categories { get; set; } = new List<CategoryPairVM>();

        [Display(Name = "Picture")]
        public string Picture { get; set; } = null!;

        [Required]
        [Display(Name = "Дата на тръгване")]
        [DataType(DataType.DateTime)]
        public DateTime DepartureTime { get; set; }

        [Required]
        [Display(Name = "Дата на пристигане")]
        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }

        [Range(0, 5000)]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Discount")]
        public decimal Discount { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
