using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly AppDbContext _context;

        public EnrollmentController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            var enrollmentViewModel = new EnrollStudentViewModel
            {
                StudentSsn = student.Ssn,
                StudentName = student.Name
            };

            ViewBag.Courses = new SelectList(await _context.Courses.ToListAsync(), "CrsId", "Name");
            return View(enrollmentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EnrollStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingEnrollment = await _context.Enrollments
                    .AnyAsync(e => e.StudentSsn == model.StudentSsn && e.CourseId == model.CourseId);

                if (existingEnrollment)
                {
                    ModelState.AddModelError("CourseId", "Student is already enrolled in this course.");
                    ViewBag.Courses = new SelectList(await _context.Courses.ToListAsync(), "CrsId", "Name", model.CourseId);
                    return View(model);
                }

                var enrollment = new Enrollment
                {
                    StudentSsn = model.StudentSsn,
                    CourseId = model.CourseId,
                    Degree = model.Degree,
                    Grade = "Final"
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Student {model.StudentName} enrolled in course successfully!";
                return RedirectToAction("Details", "Student", new { id = model.StudentSsn });
            }

            ViewBag.Courses = new SelectList(await _context.Courses.ToListAsync(), "CrsId", "Name", model.CourseId);
            return View(model);
        }
    }
}
