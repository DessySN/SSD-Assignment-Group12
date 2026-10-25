
using System.ComponentModel.DataAnnotations;

namespace SSD_LAB1.Models
{
    public class CreateMessageViewModel
    {
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        [Display(Name = "Message")]
        public string Content { get; set; } = string.Empty;
    }
}
