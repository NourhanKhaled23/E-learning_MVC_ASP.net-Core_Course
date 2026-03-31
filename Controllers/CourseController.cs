using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IMapper _mapper;

        public CourseController(ICourseRepository courseRepo, IMapper mapper)
        {
            _courseRepo = courseRepo;
            _mapper = mapper;
        }

        public IActionResult GetAll(string search = "")
        {
            var courses = _courseRepo.Search(search);
            var model = _mapper.Map<List<CourseViewModel>>(courses);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CourseTable", model);
            }

            return View(model);
        }

        public IActionResult Create()
        {
            return View(new CourseViewModel());
        }

        [HttpPost]
        public IActionResult Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existing = _courseRepo.FindByName(model.Name);
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

        public IActionResult Edit(int id)
        {
            var course = _courseRepo.GetById(id);
            if (course == null) return NotFound();

            var model = _mapper.Map<CourseViewModel>(course);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, CourseViewModel model)
        {
            if (id != model.CrsId) return NotFound();

            if (ModelState.IsValid)
            {
                var existing = _courseRepo.FindByName(model.Name);
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
    }
}
