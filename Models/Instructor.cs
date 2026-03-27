using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication1.Models
{
    public class Instructor
    {
        [Key]
        public int InsId { get; set; }

        [Required]
        public string Name { get; set; }

        public int Age { get; set; }

        public decimal Salary { get; set; }

        public string Degree { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }

        public int DeptId { get; set; }

        [ForeignKey("DeptId")]
        public Department Department { get; set; }

        public ICollection<CourseInstructor> CourseInstructors { get; set; }
    }
}