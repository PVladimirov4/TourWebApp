using System.ComponentModel.DataAnnotations;

namespace TourWebApp.Models.country
{
    public class CountryPairVM
    {
        public int Id { get; set; }

        [Display(Name = "Country")]
        public string Name { get; set; } = null!;
    }
}
