using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Models.ViewModels;
namespace WebApplication1.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileUploadService _fileUploadService;

        public StudentController(AppDbContext context, IFileUploadService fileUploadService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
        }

        public async Task<IActionResult> GetAll()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }

        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students
                .Include(s => s.Department)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.Ssn == id);

            if (student == null) return NotFound();

            var model = new StudentDetailsViewModel
            {
                Ssn = student.Ssn,
                Name = student.Name,
                Age = student.Age,
                Email = student.Email,
                Address = student.Address,
                Image = student.Image,
                DepartmentName = student.Department?.Name ?? "N/A",
                Courses = student.Enrollments.Select(e => new StudentCourseViewModel
                {
                    CourseName = e.Course.Name,
                    Degree = e.Degree,
                    MinDegree = e.Course.MinDegree
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Departments, "DeptId", "Name");
            return View(new CreateStudentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imagePath = "default-avatar.png";
                
                if (model.ImageFile != null)
                {
                    try
                    {
                        imagePath = await _fileUploadService.UploadImageAsync(model.ImageFile);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ImageFile", "Error uploading image: " + ex.Message);
                        return View(model);
                    }
                }

                var student = new Student
                {
                    Name = model.Name,
                    Age = model.Age,
                    Address = model.Address,
                    Email = model.Email,
                    Grade = model.Grade,
                    Image = imagePath
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Student created successfully!";
                return RedirectToAction("GetAll");
            }
            
            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Departments, "DeptId", "Name");
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            var model = new StudentEditViewModel
            {
                Ssn = student.Ssn,
                Name = student.Name,
                Age = student.Age,
                Address = student.Address,
                Email = student.Email,
                ExistingImage = student.Image,
                Grade = student.Grade,
                DeptId = student.DeptId
            };
            
            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Departments, "DeptId", "Name", model.DeptId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, StudentEditViewModel model)
        {
            if (id != model.Ssn) return NotFound();

            if (ModelState.IsValid)
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null) return NotFound();

                student.Name = model.Name;
                student.Age = model.Age;
                student.Address = model.Address;
                student.Email = model.Email;
                student.Grade = model.Grade;
                student.DeptId = model.DeptId;

                if (model.ImageFile != null)
                {
                    try
                    {
                        student.Image = await _fileUploadService.UploadImageAsync(model.ImageFile);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ImageFile", "Error uploading image: " + ex.Message);
                        ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Departments, "DeptId", "Name", model.DeptId);
                        return View(model);
                    }
                }

                _context.Update(student);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Student updated successfully!";
                return RedirectToAction(nameof(GetAll));
            }

            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Departments, "DeptId", "Name", model.DeptId);
            return View(model);
        }
    }
}