using HarshBulkyWeb.Data;
using HarshBulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace HarshBulkyWeb.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
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
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) { return NotFound(); }

            var categoryFromDb = _db.Categories.Find(id);
            //  var categoryFromDb1 = _db.Categories.FirstOrDefault(x=>x.CategoryId == id); //best approach
            //  var categoryFromDb3 = _db.Categories.Where(x => x.CategoryId == id).FirstOrDefault();  // only when there is any customization

            if (categoryFromDb == null) { return NotFound(); }

            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            /*  
                Note: 
                    - Have to add <input hidden asp-for="CategoryId" /> to populate id here. 
                    - If the field name is Id then there is no need to write this in the view but 
                      in my model, I have mentioned 'CategoryId' so I have to add this <input > to get the ID here. 
             
             */

            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();

            var categoryFromDb = _db.Categories.Find(obj.CategoryId);

            if (categoryFromDb == null) { return NotFound(); }

            categoryFromDb.Name = obj.Name;
            categoryFromDb.DisplayOrder = obj.DisplayOrder;

            _db.SaveChanges();

            return View();
        }


    }
}
