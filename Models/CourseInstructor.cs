using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class CourseInstructor
    {
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public required Course Course { get; set; }

        public int InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public required Instructor Instructor { get; set; }
    }
}