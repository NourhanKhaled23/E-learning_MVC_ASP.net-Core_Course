using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class StudentEditViewModel
    {
        public int Ssn { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public int Age { get; set; }
        
        public string Address { get; set; } = string.Empty;
        
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Display(Name = "Profile Image")]
        public IFormFile? ImageFile { get; set; }
        
        public string ExistingImage { get; set; } = string.Empty;
        
        public string Grade { get; set; } = string.Empty;
        
        [Display(Name = "Department")]
        public int? DeptId { get; set; }
    }
}
