using HarshBulky.DataAccess.Repository.IRepository;
using HarshBulky.Models;
using HarshBulky.Models.ViewModels;
using HarshBulky.Utility;
using Humanizer.Localisation.DateToOrdinalWords;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace HarshBulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ILogger<ProductController> logger)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
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
            // Server-side model validation - ensures data integrity before processing
            if (ModelState.IsValid)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;

                // Handling file uploads robustly
                if (file != null && file.Length > 0) // Ensure a file is actually selected and not empty
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productImagesPath = Path.Combine(webRootPath, "images", "product");

                    // Create directory if it doesn't exist - prevents potential IO exceptions
                    Directory.CreateDirectory(productImagesPath);

                    // Implementing update logic to handle existing images
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        try
                        {
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                                _logger.LogInformation($"Successfully deleted old image: {oldImagePath}");
                            }
                        }
                        catch (IOException ex)
                        {
                            _logger.LogError($"Error deleting old image: {oldImagePath}. Exception: {ex.Message}");
                            ModelState.AddModelError(string.Empty, "An error occurred while updating the image. Please try again.");
                            // Re-populate CategoryList and return the view with the error
                            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.CategoryId.ToString() });
                            return View(productVM);
                        }
                    }

                    string filePath = Path.Combine(productImagesPath, fileName);

                    // Saving the new file using a try-catch block for error handling
                    try
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        productVM.Product.ImageUrl = Path.Combine("\\images\\product\\", fileName); // Using Path.Combine for platform-independent path construction
                        _logger.LogInformation($"Successfully saved image: {filePath}");
                    }
                    catch (IOException ex)
                    {
                        _logger.LogError($"Error saving image: {filePath}. Exception: {ex.Message}");
                        ModelState.AddModelError(string.Empty, "An error occurred while saving the image. Please try again.");
                        // Re-populate CategoryList and return the view with the error
                        productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.CategoryId.ToString() });
                        return View(productVM);
                    }
                }
                else if (productVM.Product.Id == 0)
                {
                    // If creating a new product and no file is uploaded, ensure ImageUrl isn't accidentally set
                    productVM.Product.ImageUrl = null;
                }
                // For update scenarios where the user might not upload a new image, we retain the existing ImageUrl

                // Centralized data persistence logic
                try
                {
                    if (productVM.Product.Id == 0)
                    {
                        _unitOfWork.Product.Add(productVM.Product);
                        _logger.LogInformation($"Product with ID {productVM.Product.Title} added successfully.");
                    }
                    else
                    {
                        _unitOfWork.Product.Update(productVM.Product);
                        _logger.LogInformation($"Product with ID {productVM.Product.Id} updated successfully.");
                    }

                    _unitOfWork.Save(); // Consider implementing asynchronous SaveChangesAsync in a production environment
                    TempData["success"] = "Product saved successfully!"; // Using a more generic message

                    return RedirectToAction(nameof(Index)); // Using nameof for better refactoring support
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error saving product to the database. Exception: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the product. Please try again.");
                    // Re-populate CategoryList and return the view with the error
                    productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.CategoryId.ToString() });
                    return View(productVM);
                }
            }
            else
            {
                // If ModelState is invalid, re-populate the CategoryList for the view
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.CategoryId.ToString() });
                return View(productVM); // Return the model with validation errors
            }
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
