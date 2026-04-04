using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InstructorController : Controller
    {
        private readonly IRepository<Instructor> _instructorRepo;
        private readonly IRepository<Department> _deptRepo;

        public InstructorController(IRepository<Instructor> instructorRepo, IRepository<Department> deptRepo)
        {
            _instructorRepo = instructorRepo;
            _deptRepo = deptRepo;
        }

        public IActionResult Index()
        {
            var instructors = _instructorRepo.GetAll("Department");
            return View(instructors);
        }

        public IActionResult Create()
        {
            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_deptRepo.GetAll(), "DeptId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Age,Salary,Degree,Email,Address,DeptId")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _instructorRepo.Add(instructor);
                TempData["Success"] = "Instructor added successfully!";
                return RedirectToAction("Index");
            }
            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_deptRepo.GetAll(), "DeptId", "Name");
            return View(instructor);
        }

        public IActionResult Edit(int id)
        {
            var instructor = _instructorRepo.GetById(id);
            if (instructor == null) return NotFound();
            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_deptRepo.GetAll(), "DeptId", "Name", instructor.DeptId);
            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("InsId,Name,Age,Salary,Degree,Email,Address,DeptId")] Instructor instructor)
        {
            if (id != instructor.InsId) return NotFound();

            if (ModelState.IsValid)
            {
                _instructorRepo.Update(instructor);
                TempData["Success"] = "Instructor updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_deptRepo.GetAll(), "DeptId", "Name", instructor.DeptId);
            return View(instructor);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var instructor = _instructorRepo.GetById(id);
            if (instructor == null) return NotFound();
            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _instructorRepo.Delete(id);
            TempData["Success"] = "Instructor deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
