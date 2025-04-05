using HarshBulky.DataAccess.Data;
using HarshBulky.DataAccess.Repository.IRepository;
using HarshBulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace HarshBulkyWeb.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Category Name and Display Order cannot be same.");
            }

            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category Created Successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) { return NotFound(); }

            var categoryFromDb = _categoryRepo.Get(x => x.CategoryId == id);
            //  var categoryFromDb1 = _db.Categories.FirstOrDefault(x=>x.CategoryId == id); //best approach
            //  var categoryFromDb3 = _db.Categories.Where(x => x.CategoryId == id).FirstOrDefault();  // only when there is any customization

            if (categoryFromDb == null) { return NotFound(); }

            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category Updated Successfully!";
                return RedirectToAction("Index");

            }
            return View();
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) { return NotFound(); }

            var categoryFromDb = _categoryRepo.Get(x => x.CategoryId == id);
            //  var categoryFromDb1 = _db.Categories.FirstOrDefault(x=>x.CategoryId == id); //best approach
            //  var categoryFromDb3 = _db.Categories.Where(x => x.CategoryId == id).FirstOrDefault();  // only when there is any customization

            if (categoryFromDb == null) { return NotFound(); }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category obj = _categoryRepo.Get(x => x.CategoryId == id);

            if (obj == null) { return NotFound(); }

            _categoryRepo.Remove(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category Deleted Successfully!";
            return RedirectToAction("Index");

        }

    }
}
