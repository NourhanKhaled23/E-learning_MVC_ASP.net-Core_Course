using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly ICourseRepository _courseRepo;

        public EnrollmentController(IEnrollmentRepository enrollmentRepo, IStudentRepository studentRepo, ICourseRepository courseRepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _studentRepo = studentRepo;
            _courseRepo = courseRepo;
        }

        public IActionResult Create(int id)
        {
            var student = _studentRepo.GetById(id);
            if (student == null) return NotFound();

            var enrollmentViewModel = new EnrollStudentViewModel
            {
                StudentSsn = student.Ssn,
                StudentName = student.Name
            };

            ViewBag.Courses = new SelectList(_courseRepo.GetAll(), "CrsId", "Name");
            return View(enrollmentViewModel);
        }

        [HttpPost]
        public IActionResult Create(EnrollStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_enrollmentRepo.Exists(model.StudentSsn, model.CourseId))
                {
                    ModelState.AddModelError("CourseId", "Student is already enrolled in this course.");
                    ViewBag.Courses = new SelectList(_courseRepo.GetAll(), "CrsId", "Name", model.CourseId);
                    return View(model);
                }

                var enrollment = new Enrollment
                {
                    StudentSsn = model.StudentSsn,
                    CourseId = model.CourseId,
                    Degree = model.Degree,
                    Grade = "Final"
                };

                _enrollmentRepo.Add(enrollment);

                TempData["Success"] = $"Student {model.StudentName} enrolled in course successfully!";
                return RedirectToAction("Details", "Student", new { id = model.StudentSsn });
            }

            ViewBag.Courses = new SelectList(_courseRepo.GetAll(), "CrsId", "Name", model.CourseId);
            return View(model);
        }
    }
}
