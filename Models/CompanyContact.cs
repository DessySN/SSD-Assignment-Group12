// 
//This model stores contact people associated with companies.
//For example, a company might have a manager or sales representative.
// />
using System.ComponentModel.DataAnnotations;

namespace SSD_LAB1.Models
{
    public class CompanyContact
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(25)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public Company? Company { get; set; }
    }
}
