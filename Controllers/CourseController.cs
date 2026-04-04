using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly IRepository<Course> _courseRepo;
        private readonly IMapper _mapper;

        public CourseController(IRepository<Course> courseRepo, IMapper mapper)
        {
            _courseRepo = courseRepo;
            _mapper = mapper;
        }

        public IActionResult GetAll(string search = "")
        {
            var courses = string.IsNullOrWhiteSpace(search) ? _courseRepo.GetAll() : _courseRepo.Find(c => c.Name.Contains(search) || (c.Description != null && c.Description.Contains(search)));
            var model = _mapper.Map<List<CourseViewModel>>(courses);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CourseTable", model);
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new CourseViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existing = _courseRepo.GetFirstOrDefault(c => c.Name.ToLower() == model.Name.ToLower());
                if (existing != null)
                {
                    ModelState.AddModelError("Name", "A course with this name already exists.");
                    return View(model);
                }

                var course = _mapper.Map<Course>(model);
                _courseRepo.Add(course);
                
                TempData["Success"] = "Course added successfully!";
                return RedirectToAction(nameof(GetAll));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var course = _courseRepo.GetById(id);
            if (course == null) return NotFound();

            var model = _mapper.Map<CourseViewModel>(course);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CourseViewModel model)
        {
            if (id != model.CrsId) return NotFound();

            if (ModelState.IsValid)
            {
                var existing = _courseRepo.GetFirstOrDefault(c => c.Name.ToLower() == model.Name.ToLower());
                if (existing != null && existing.CrsId != id)
                {
                    ModelState.AddModelError("Name", "Another course with this name already exists.");
                    return View(model);
                }

                var course = _mapper.Map<Course>(model);
                _courseRepo.Update(course);
                
                TempData["Success"] = "Course updated successfully!";
                return RedirectToAction(nameof(GetAll));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var course = _courseRepo.GetById(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _courseRepo.Delete(id);
            TempData["Success"] = "Course deleted successfully!";
            return RedirectToAction(nameof(GetAll));
        }
    }
}
