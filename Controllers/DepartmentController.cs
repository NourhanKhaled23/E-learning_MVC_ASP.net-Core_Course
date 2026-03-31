using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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

        public IActionResult Edit(int id)
        {
            var department = _deptRepo.GetById(id);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost]
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


        public IActionResult AddV2()
        {
            return View();
        }


        [HttpPost]
        [ServiceFilter(typeof(ValidateLocationFilter))]
        public IActionResult AddV2(Department department)
        {
            if (!ModelState.IsValid)
                return View(department);


            _deptRepo.Add(department);
            return RedirectToAction("GetAll");
        }
    }
}