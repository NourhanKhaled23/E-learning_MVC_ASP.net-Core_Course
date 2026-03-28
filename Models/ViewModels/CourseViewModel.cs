using System.ComponentModel.DataAnnotations;
using WebApplication1.Validation;

namespace WebApplication1.Models.ViewModels
{
    public class CourseViewModel
    {
        public int CrsId { get; set; }

        [Required(ErrorMessage = "Course Name is required.")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Topics { get; set; } = string.Empty;

        [Required(ErrorMessage = "Minimum passing degree is required.")]
        [Range(1, 200, ErrorMessage = "Minimum passing degree must be between 1 and 200.")]
        [LessThan("FullDegree", ErrorMessage = "Min degree cannot be greater than or equal to the total course degree.")]
        public double MinDegree { get; set; }

        [Required(ErrorMessage = "Total course degree is required.")]
        [Range(1, 200, ErrorMessage = "Total degree must be between 1 and 200.")]
        public double FullDegree { get; set; }
    }
}
