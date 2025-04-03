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
    }
}
