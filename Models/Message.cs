
using System.ComponentModel.DataAnnotations;

namespace SSD_LAB1.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        [Display(Name = "Message")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Posted At")]
        public DateTime PostedAt { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;

        public ApplicationUser? ApplicationUser { get; set; }
    }
}
