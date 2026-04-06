using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourWebApp.Infrastructure.Data.Entities
{
    [PrimaryKey(nameof(HolidayId), nameof(UserId))]
    public class Favorite
    {
        [Required]
        public int HolidayId { get; set; }
        public virtual Holiday Holiday { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
