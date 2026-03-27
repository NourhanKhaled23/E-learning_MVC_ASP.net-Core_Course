using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetAll()
        {
            var departments = await _context.Departments
                .Include(d => d.Students)
                .Include(d => d.Instructors)
                .ToListAsync();
            return View(departments);
        }

        public async Task<IActionResult> Details(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Students)
                .Include(d => d.Instructors)
                .FirstOrDefaultAsync(d => d.DeptId == id);
            
            if (department == null)
                return NotFound();
            
            return View(department);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Department added successfully!";
                return RedirectToAction("GetAll");
            }
            
            return View(department);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.DeptId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(department);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Department updated successfully!";
                return RedirectToAction(nameof(GetAll));
            }
            return View(department);
        }
    }
}