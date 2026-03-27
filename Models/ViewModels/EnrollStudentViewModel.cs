using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class EnrollStudentViewModel
    {
        [Required]
        public int StudentSsn { get; set; }
        
        public string StudentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a course.")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Degree is required.")]
        [Range(0, 100, ErrorMessage = "Degree must be between 0 and 100.")]
        public double Degree { get; set; }
        
        public string Grade { get; set; } = string.Empty;
    }
}
