using System.ComponentModel.DataAnnotations;

namespace DormProject.DormOffice.Data.Models
{
    public class StudentRequestDTO
    {
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        public double Grade { get; set; }
    }
}
