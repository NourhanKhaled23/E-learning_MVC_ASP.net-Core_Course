namespace WebApplication1.Models.ViewModels
{
    public class StudentCourseViewModel
    {
        public string CourseName { get; set; } = string.Empty;
        public double Degree { get; set; }
        public double MinDegree { get; set; }
        
        public bool IsPassing => Degree >= MinDegree;
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
