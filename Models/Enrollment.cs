using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Enrollment
    {
        public int StudentSsn { get; set; }

        [ForeignKey("StudentSsn")]
        public Student? Student { get; set; }

        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        public string Grade { get; set; } = string.Empty;
        
        public double Degree { get; set; }
    }
}