using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Student
    {
        [Key]
        public int Ssn { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Address { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public string Grade { get; set; } = string.Empty;

        public int? DeptId { get; set; }

        [ForeignKey("DeptId")]
        public Department? Department { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = [];
    }
}