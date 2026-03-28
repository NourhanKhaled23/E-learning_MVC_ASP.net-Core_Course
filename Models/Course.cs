
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Course
    {
        [Key]
        public int CrsId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Topics { get; set; } = string.Empty;

        [Required(ErrorMessage = "Minimum degree required is mandatory")]
        [Range(20, 100, ErrorMessage = "Minimum passing degree must be between 20 and 100")]
        public double MinDegree { get; set; }

        [Required(ErrorMessage = "Total course degree is required")]
        [Range(1, 200, ErrorMessage = "Course total degree must be between 1 and 200")]
        public double FullDegree { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = [];
        public ICollection<CourseInstructor> CourseInstructors { get; set; } = [];
    }
}