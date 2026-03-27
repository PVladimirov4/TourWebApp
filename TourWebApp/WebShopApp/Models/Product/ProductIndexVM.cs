using System.ComponentModel.DataAnnotations;

namespace TourWebApp.Models.Product
{
    public class ProductIndexVM
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }

        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [Required]
        [Display(Name = "Дата на тръгване")]
        [DataType(DataType.DateTime)]
        public DateTime DepartureTime { get; set; }

        [Required]
        [Display(Name = "Дата на пристигане")]
        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }

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
