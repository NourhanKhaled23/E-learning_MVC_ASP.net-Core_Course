using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class CourseViewModel
    {
        public int CrsId { get; set; }

        [Required(ErrorMessage = "Course Name is required.")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Topics { get; set; } = string.Empty;

        [Required(ErrorMessage = "Minimum degree is required.")]
        [Range(1, 100, ErrorMessage = "Minimum passing degree must be between 1 and 100.")]
        public double MinDegree { get; set; }
    }
}
