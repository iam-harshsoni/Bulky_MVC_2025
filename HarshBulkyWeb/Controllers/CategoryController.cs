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
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Category Name and Display Order cannot be same.");
            }
            if(obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "Test is an invalid Value.");
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }
    }
}
