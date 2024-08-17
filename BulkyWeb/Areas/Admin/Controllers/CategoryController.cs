using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor Region

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }


        #endregion

        #region Gets Region

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult TestApi()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return Json(objCategoryList);
        }

        #endregion

        #region Createtion Region
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannt exactly match the Name");
            }
            if (category.Name == "test")
            {
                ModelState.AddModelError("", "Test is an invaild value");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfuly";
                return RedirectToAction("Index", "Category");
            }
            return View();


        }


        #endregion

        #region Edit Region

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = _unitOfWork.Category.Get(u => u.Id == id);
            //Category? category1 = _db.Categories.FirstOrDefault(u =>u.Id ==id);
            //Category? category3 = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannt exactly match the Name");
            }
            if (category.Name == "test")
            {
                ModelState.AddModelError("", "Test is an invaild value");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfuly";
                return RedirectToAction("Index", "Category");
            }
            return View();


        }

        #endregion

        #region Delete Region
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = _unitOfWork.Category.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {

            Category? category = _unitOfWork.Category.Get(u => u.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfuly";
            return RedirectToAction("index", "Category");
        }

        #endregion

    }

}
