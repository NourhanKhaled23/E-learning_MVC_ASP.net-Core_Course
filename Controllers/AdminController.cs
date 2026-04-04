using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.StudentCount = _context.Students.Count();
            ViewBag.InstructorCount = _context.Instructors.Count();
            ViewBag.CourseCount = _context.Courses.Count();
            ViewBag.DepartmentCount = _context.Departments.Count();
            
            return View();
        }
    }
}
