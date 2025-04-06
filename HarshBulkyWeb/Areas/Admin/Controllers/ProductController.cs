using HarshBulky.DataAccess.Repository.IRepository;
using HarshBulky.Models;
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

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll().ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            /*  
              Projections in EF Core. Very Powerfull feature 
              Coverting IEnumerable<Category> to IEnumerable<SelectListItem> dynamically in single command.
           */

            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().
                Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                });

            ViewBag.CategoryList = categoryList;  //using ViewBag to send the categoryList to view.
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully!";

                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Product product = _unitOfWork.Product.Get(u => u.Id == id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully!";

                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            Product product = _unitOfWork.Product.Get(u => u.Id == id);
            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteProduct(int id)
        {
            if (ModelState.IsValid)
            {
                Product product = _unitOfWork.Product.Get(u => u.Id == id);

                if (product==null)  return NotFound();

                _unitOfWork.Product.Remove(product);
                _unitOfWork.Save();
                TempData["success"] = "Product deleted successfully!";

                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
