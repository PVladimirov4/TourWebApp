using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unipluss.Sign.ExternalContract.Entities;

namespace TourWebApp.Infrastructure.Data.Entities
{
    public class Country
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]

        public string CountryName { get; set; } = null!;
        public virtual IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}
