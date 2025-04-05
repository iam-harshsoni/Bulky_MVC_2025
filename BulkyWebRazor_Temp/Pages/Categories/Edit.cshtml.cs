using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    public class EditModel(ApplicationDbContext db) : PageModel
    {
        private readonly ApplicationDbContext _db = db;
        
        [BindProperty]
        public required Category Category { get; set; }

        public void OnGet(int id)
        {
            Category = _db.Categories.FirstOrDefault(x=>x.CategoryId == id);
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(Category);
                _db.SaveChanges();
                TempData["success"] = "Category Updated Successfully!";
                return RedirectToPage("Index");
            }
            return RedirectToPage("Edit");
        }
    }
}
