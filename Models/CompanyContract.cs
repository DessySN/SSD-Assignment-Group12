
using System.ComponentModel.DataAnnotations;

namespace SSD_LAB1.Models
{
    public class CompanyContract : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Contract Number")]
        public string ContractNumber { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Range(typeof(decimal), "0.01", "999999999.99")]
        [DataType(DataType.Currency)]
        [Display(Name = "Contract Value")]
        public decimal ContractValue { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public Company? Company { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (EndDate.Date < StartDate.Date)
            {
                yield return new ValidationResult(
                    "End Date must be on or after Start Date.",
                    new[] { nameof(EndDate) });
            }
        }
    }
}
