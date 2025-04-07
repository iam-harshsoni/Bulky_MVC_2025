using HarshBulky.DataAccess.Repository.IRepository;
using HarshBulky.Models;
using HarshBulky.Models.ViewModels;
using Humanizer.Localisation.DateToOrdinalWords;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace HarshBulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll("Category").ToList();
            return View(products);
        }

        public IActionResult Upsert(int? id)  //Upsert = Update and Insert
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().
                Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                }),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                // Insert / Create View
                return View(productVM);
            }
            else
            {
                //Update View

                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                // Get the absolute path to the web root directory (e.g., wwwroot).
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // Check if a file was uploaded.
                if (file != null)
                {
                    // Generate a unique file name to avoid naming conflicts.
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    // Define the path to the product images directory within the web root.
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    // Check if an existing image URL is present (for update scenarios).
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        // Construct the full path to the old image file.
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        // Check if the old image file exists.
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            // Delete the old image file from the file system.
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Create the full path to save the new image file.
                    string filePath = Path.Combine(productPath, fileName);

                    // Save the uploaded file to the specified directory.
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Update the ProductViewModel with the relative URL of the saved image.
                    // This URL will be stored in the database.
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                // Check if it's a new product (Id is 0) or an existing product being updated.
                if (productVM.Product.Id == 0)
                {
                    // Add the new product to the database.
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    // Update the existing product in the database.
                    _unitOfWork.Product.Update(productVM.Product);
                }

                // Save all changes made to the database.
                _unitOfWork.Save();

                // Store a success message in TempData to be displayed on the next page load.
                TempData["success"] = "Product created successfully!";

                // Redirect the user to the Index action (likely the product listing page).
                return RedirectToAction("Index");
            }
            else
            {
                // If the model state is not valid (validation errors occurred),
                // repopulate the CategoryList for the view.
                productVM.CategoryList = _unitOfWork.Category.GetAll().
                    Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.CategoryId.ToString()
                    });

                // Return the ProductViewModel back to the view so the user can see the validation errors
                // and correct the input.
                return View(productVM);
            }

            // This line should ideally not be reached if the logic above is correct.
            // It's likely a fallback in case of unexpected behavior.
            return View();
        }

        #region API Call

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.Product.GetAll("Category").ToList();
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {

            Product productToBeDeleted = _unitOfWork.Product.Get(x => x.Id == id);
            if (productToBeDeleted == null) return Json(new { success = false, message = "Error while deleting." });

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = false, message = "Delete successfull!" });
        }

        #endregion

    }
}
