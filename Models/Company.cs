
using System.ComponentModel.DataAnnotations;

namespace SSD_LAB1.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [Display(Name = "Years In Business")]
        public int YearsInBusiness { get; set; }

        [Required]
        [Url]
        public required string Website { get; set; }

        public string? Province { get; set; }
    }
}
