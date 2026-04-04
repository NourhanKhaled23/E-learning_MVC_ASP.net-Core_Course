using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IRepository<Department> _deptRepo;

        public DepartmentController(IRepository<Department> deptRepo)
        {
            _deptRepo = deptRepo;
        }

        public IActionResult GetAll()
        {
            var departments = _deptRepo.GetAll("Students", "Instructors");
            return View(departments);
        }

        public IActionResult Details(int id)
        {
            var department = _deptRepo.GetFirstOrDefault(d => d.DeptId == id, "Students", "Instructors");
            
            if (department == null)
                return NotFound();
            
            return View(department);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _deptRepo.Add(department);
                
                TempData["Success"] = "Department added successfully!";
                return RedirectToAction("GetAll");
            }
            
            return View(department);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var department = _deptRepo.GetById(id);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Department department)
        {
            if (id != department.DeptId) return NotFound();

            if (ModelState.IsValid)
            {
                _deptRepo.Update(department);
                
                TempData["Success"] = "Department updated successfully!";
                return RedirectToAction(nameof(GetAll));
            }
            return View(department);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult AddV2()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(ValidateLocationFilter))]
        public IActionResult AddV2(Department department)
        {
            if (!ModelState.IsValid)
                return View(department);


            _deptRepo.Add(department);
            return RedirectToAction("GetAll");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var department = _deptRepo.GetById(id);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _deptRepo.Delete(id);
            TempData["Success"] = "Department deleted successfully!";
            return RedirectToAction(nameof(GetAll));
        }
    }
}