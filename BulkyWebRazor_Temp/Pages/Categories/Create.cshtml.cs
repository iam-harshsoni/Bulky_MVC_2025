using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]  // Also use this instead of [BindProperty] if we have more than one properties to bind.
    public class CreateModel(ApplicationDbContext db) : PageModel
    {
        private readonly ApplicationDbContext _db = db;

        /*
            `BindProperty` in Razor Pages automatically binds form data to the property, 
            so you don’t need to manually retrieve values from the request. It simplifies 
            data handling, making it easy to process user input in your page model.
         */

        [BindProperty]
        public required Category Category { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(Category);
                _db.SaveChanges();
                TempData["success"] = "Category Created Successfully!";
                return RedirectToPage("Index");
            }
            return RedirectToPage("Create");
        }
    }
}
