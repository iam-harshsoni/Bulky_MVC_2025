using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    public class DeleteModel(ApplicationDbContext db) : PageModel
    {
        [BindProperty]
        public required Category Category {get; set;}

        private ApplicationDbContext _db = db;

        public void OnGet(int id)
        {
            Category = _db.Categories.FirstOrDefault(x => x.CategoryId == id);
        }

        public IActionResult OnPost()
        {
            try
            {
                _db.Categories.Remove(Category);
                _db.SaveChanges();
                TempData["success"] = "Category Deleted Successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the category.");

            }

            return Page();

        }
    }
}
