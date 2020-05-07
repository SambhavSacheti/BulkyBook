using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (!id.HasValue)
            {
                // For Create 
                return View(category);
            }
            category = unitOfWork.Category.Get(id.GetValueOrDefault());
            // For edit 
            return category == null ?
                                    (IActionResult)NotFound()   // Category don't exist in the database
                                    : View(category);           // return the category 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);
            if (category.Id == 0)
            {
                unitOfWork.Category.Add(category);
            }
            else
            {
                unitOfWork.Category.Update(category);
            }
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var category = unitOfWork.Category.Get(id);
            if (category == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error wile deleting."
                });
            }

            unitOfWork.Category.Remove(category);
            unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = $"{category.Name} is deleted."
            });
        }


        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = unitOfWork.Category.GetAll();
            return Json(new { data = allObj });
        }
        #endregion

    }

}