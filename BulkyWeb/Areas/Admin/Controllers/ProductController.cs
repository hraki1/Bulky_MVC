using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
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
            IEnumerable<Product> products = _unitOfWork.Product.GetAll();
            return View(products);
        }
        public IActionResult Create()
        {

            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;

            ProductVM productVM = new ProductVM()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),

                })
,
                Product = new()
            };

            return View(productVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVM.Product);
                TempData["success"] = "Product created successfuly";

                _unitOfWork.Save();
                return RedirectToAction("Index");

            }
            else
            {

                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),

                });

                return View(productVM);
            }
        }
        public IActionResult Edit(int? id)
        {

            var category = _unitOfWork.Product.Get(u => u.Id == id);
            return View(category);

        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                return RedirectToAction("Index");

            }

            return View();
        }
        public IActionResult Delete(int? id)
        {
            var product = _unitOfWork.Product.Get(u => u.Id == id);
            return View(product);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            var product = _unitOfWork.Product.Get(u => u.Id == id);
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }


    }
}
