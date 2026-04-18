using System.ComponentModel.DataAnnotations;
using TourWebApp.Models.country;
using TourWebApp.Models.Category;

namespace TourWebApp.Models.Holiday
{
    public class HolidayCreateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името е задължително")]
        [MaxLength(30)]
        [Display(Name = "Име на почивката")]
        public string HolidayName { get; set; } = null!;

        [Required(ErrorMessage = "Изберете държава")]
        [Display(Name = "Държава")]
        public int CountryId { get; set; }
        
        // Махаме изискването за списъците
        public virtual List<CountryPairVM>? Countrys { get; set; } = new List<CountryPairVM>();

        [Required(ErrorMessage = "Изберете категория")]
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        public virtual List<CategoryPairVM>? Categories { get; set; } = new List<CategoryPairVM>();

        [Required(ErrorMessage = "Снимката е задължителна")]
        [Display(Name = "Линк към снимка")]
        public string Picture { get; set; } = null!;

        [Required]
        [Display(Name = "Заминаване")]
        public DateTime DepartureTime { get; set; }

        [Required]
        [Display(Name = "Връщане")]
        public DateTime ArrivalDate { get; set; }

        [Range(1, 5000, ErrorMessage = "Местата трябва да са между 1 и 5000")]
        public int Quantity { get; set; }

        [Range(0.01, 100000, ErrorMessage = "Цената трябва да е положително число")]
        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Описанието е задължително")]
        public string Description { get; set; } = null!;
    }
}