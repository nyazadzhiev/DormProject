using System.ComponentModel.DataAnnotations;

namespace DormProject.Identity.Models
{
    public class UserInputModel
    {
        [EmailAddress]
        [Required]
        [MinLength(4)]
        [MaxLength(320)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
