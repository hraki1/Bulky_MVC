using Bulky.DataAccess.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
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
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category created successfuly";
                return RedirectToAction("Index", "Category");
            }
            return View();


        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = _db.Categories.Find(id);
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
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfuly";
                return RedirectToAction("Index", "Category");
            }
            return View();


        }

        #region Delete Region
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {

            Category? category = _db.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfuly";
            return RedirectToAction("index", "Category");
        }

        #endregion

    }

}
