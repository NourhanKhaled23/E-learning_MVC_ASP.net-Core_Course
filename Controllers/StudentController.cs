using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repositories;
using WebApplication1.Filters;
using System.Threading.Tasks;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IRepository<Student> _studentRepo;
        private readonly IRepository<Department> _deptRepo;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(
            IRepository<Student> studentRepo, 
            IRepository<Department> deptRepo, 
            IFileUploadService fileUploadService, 
            ILogger<StudentController> logger)
        {
            _studentRepo = studentRepo;
            _deptRepo = deptRepo;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin, Instructor")]
        public IActionResult GetAll()
        {
            var students = _studentRepo.GetAll();

            ViewData["Title"] = "Student List";
            ViewData["ServerTime"] = DateTime.Now;

            ViewBag.StudentCount = students.Count();

            ViewBag.LastViewedStudent = HttpContext.Session.GetString("LastViewedStudent");

            return View(students);
        }

        public IActionResult Details(int id)
        {
            var student = _studentRepo.GetFirstOrDefault(s => s.Ssn == id, "Department", "Enrollments.Course");

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
                    CourseId = e.CourseId,
                    CourseName = e.Course?.Name ?? "Unknown",
                    Degree = e.Degree,
                    MinDegree = e.Course?.MinDegree ?? 0
                }).ToList()
            };
            
            HttpContext.Session.SetString("LastViewedStudent", student.Name);

            return View(model);
        }

        public IActionResult SessionInfo()
        {
            var lastStudent = HttpContext.Session.GetString("LastViewedStudent") ?? "No student viewed yet!";
            ViewBag.LastStudent = lastStudent;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_deptRepo.GetAll(), "DeptId", "Name");
            return View(new CreateStudentViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateStudentViewModel model,
            [FromKeyedServices("email")] INotificationService emailNotificationService,
            [FromKeyedServices("sms")] INotificationService smsNotificationService)
        {
            if (ModelState.IsValid)
            {
                string imagePath = "default-avatar.png";
                
                if (model.ImageFile != null)
                {
                    try
                    {
                        var uploadedImage = await _fileUploadService.UploadImageAsync(model.ImageFile);
                        if (uploadedImage != null)
                        {
                            imagePath = uploadedImage;
                        }
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
                    Image = imagePath,
                    DeptId = model.DeptId
                };

                _studentRepo.Add(student);
                
                _logger.LogInformation($"Student added successfully: {student.Name} (SSN: {student.Ssn})");
                
                await emailNotificationService.SendAsync(student.Email ?? "Unknown", $"Welcome {student.Name}, your student account has been created!");
                await smsNotificationService.SendAsync(student.Name, "Your student registration was successful!");
                
                TempData["Success"] = "Student added successfully!";
                
                return RedirectToAction("GetAll");
            }
            
            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_deptRepo.GetAll(), "DeptId", "Name");
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var student = _studentRepo.GetById(id);
            if (student == null) return NotFound();

            var model = new StudentEditViewModel
            {
                Ssn = student.Ssn,
                Name = student.Name,
                Age = student.Age,
                Address = student.Address,
                Email = student.Email,
                ExistingImage = student.Image,
                DeptId = student.DeptId
            };
            
            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_deptRepo.GetAll(), "DeptId", "Name", model.DeptId);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentEditViewModel model)
        {
            if (id != model.Ssn) return NotFound();

            if (ModelState.IsValid)
            {
                var student = _studentRepo.GetById(id);
                if (student == null) return NotFound();

                student.Name = model.Name;
                student.Age = model.Age;
                student.Address = model.Address;
                student.Email = model.Email;
                student.DeptId = model.DeptId;

                if (model.ImageFile != null)
                {
                    try
                    {
                        var uploadedImage = await _fileUploadService.UploadImageAsync(model.ImageFile);
                        if (uploadedImage != null)
                        {
                            student.Image = uploadedImage;
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ImageFile", "Error uploading image: " + ex.Message);
                        ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_deptRepo.GetAll(), "DeptId", "Name", model.DeptId);
                        return View(model);
                    }
                }

                _studentRepo.Update(student);
                
                TempData["Success"] = "Student updated successfully!";
                return RedirectToAction(nameof(GetAll));
            }

            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_deptRepo.GetAll(), "DeptId", "Name", model.DeptId);
            return View(model);
        }

        [Authorize(Roles = "Student")]
        public IActionResult MyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var student = _studentRepo.GetFirstOrDefault(s => s.ApplicationUserId == userId, "Department", "Enrollments.Course");
            if (student == null) return NotFound("Student profile not found. Please contact administration.");

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
                    CourseId = e.CourseId,
                    CourseName = e.Course?.Name ?? "Unknown",
                    Degree = e.Degree,
                    MinDegree = e.Course?.MinDegree ?? 0
                }).ToList()
            };

            return View("Details", model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var student = _studentRepo.GetById(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _studentRepo.GetById(id);
            if (student != null)
            {
                if (!string.IsNullOrEmpty(student.Image) && student.Image != "default-avatar.png" && student.Image != "placeholder.svg")
                {
                    _fileUploadService.DeleteImage(student.Image);
                }

                _studentRepo.Delete(id);
                TempData["Success"] = "Student deleted successfully!";
            }
            return RedirectToAction(nameof(GetAll));
        }
    }
}