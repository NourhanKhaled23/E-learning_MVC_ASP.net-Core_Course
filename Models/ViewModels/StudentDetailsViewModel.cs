namespace WebApplication1.Models.ViewModels
{
    public class StudentCourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public double Degree { get; set; }
        public double MinDegree { get; set; }
        
        public bool IsPassing => Degree >= MinDegree;
        
        public string StatusText => Degree switch
        {
            var d when d < MinDegree => "Fail",
            var d when d >= 85 => "Excellent",
            var d when d >= 75 => "Very Good",
            var d when d >= 65 => "Good",
            _ => "Pass"
        };
        
        public string BadgeClass => Degree switch
        {
            var d when d < MinDegree => "bg-danger",
            var d when d >= 85 => "bg-success",
            var d when d >= 75 => "bg-info text-white",
            var d when d >= 65 => "bg-theme text-white",
            _ => "bg-warning text-dark"
        };
    }

    public class StudentDetailsViewModel
    {
        public int Ssn { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        
        public List<StudentCourseViewModel> Courses { get; set; } = new();
    }
}
