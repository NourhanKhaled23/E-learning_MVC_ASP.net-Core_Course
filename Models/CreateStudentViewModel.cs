using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class CreateStudentViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Address { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Profile Image")]
        public IFormFile? ImageFile { get; set; }

        public string Grade { get; set; } = string.Empty;
    }
}