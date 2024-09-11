using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SqlServer.Server;
using System.IO;

namespace BulkyBookWeb.Areas.Admin.Controllers
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
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties:"Category");
            return View(products);
        }
        public IActionResult Upsert(int? id)
        {

            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;

            ProductVM productVM = new ProductVM()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                }),

                Product = new()
            };
            if (id == null || id == 0)
            {
                // Create
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);

            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            Console.WriteLine(file);

            if (ModelState.IsValid)
            {
                //Get Root Path
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                // Check if file is Empty Or not
                if (file != null)
                {
                    // This Will give us Random name to our File + with extension of this file
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Specify Path location where have to save our file
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    //check if there is exsisting ImageUrl ofr update action
                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        // delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Opens a file stream for the file at the path formed by combining productpath and fileName.
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    // Add full path to ImageUrl Column
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }


                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
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
