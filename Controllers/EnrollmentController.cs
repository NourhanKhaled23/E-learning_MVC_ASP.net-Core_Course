using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin, Instructor")]
    public class EnrollmentController : Controller
    {
        private readonly IRepository<Enrollment> _enrollmentRepo;
        private readonly IRepository<Student> _studentRepo;
        private readonly IRepository<Course> _courseRepo;

        public EnrollmentController(IRepository<Enrollment> enrollmentRepo, IRepository<Student> studentRepo, IRepository<Course> courseRepo)
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(EnrollStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_enrollmentRepo.Find(e => e.StudentSsn == model.StudentSsn && e.CourseId == model.CourseId).Any())
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

        public IActionResult Delete(int studentSsn, int courseId)
        {
            var enrollment = _enrollmentRepo.GetFirstOrDefault(e => e.StudentSsn == studentSsn && e.CourseId == courseId, "Student", "Course");
            if (enrollment == null) return NotFound();
            return View(enrollment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int studentSsn, int courseId)
        {
            var enrollment = _enrollmentRepo.GetFirstOrDefault(e => e.StudentSsn == studentSsn && e.CourseId == courseId);
            if (enrollment != null)
            {
                _enrollmentRepo.Delete(enrollment);
                TempData["Success"] = "Student unenrolled successfully!";
            }
            return RedirectToAction("Details", "Student", new { id = studentSsn });
        }
    }
}
