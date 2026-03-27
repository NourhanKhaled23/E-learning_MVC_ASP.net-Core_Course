
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

        public double MinDegree { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = [];
        public ICollection<CourseInstructor> CourseInstructors { get; set; } = [];
    }
}