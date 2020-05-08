using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if (!id.HasValue)
            {
                return View(coverType);
            }
            coverType = unitOfWork.CoverType.Get(id.GetValueOrDefault());
            return coverType == null ?
                           (IActionResult)NotFound()
                         : View(coverType);
        }

        [HttpPost]
        public IActionResult Upsert(CoverType coverType)
        {
              if (!ModelState.IsValid)
                return View(coverType);
                if(coverType.Id ==0)
                     unitOfWork.CoverType.Add(coverType);
                else
                    unitOfWork.CoverType.Update(coverType);

               unitOfWork.Save();
               return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var coverType = unitOfWork.CoverType.Get(id);
            if(coverType == null)
                return  Json(new
                 {
                    success = false,
                    message="Cover Type not found"
                });
            unitOfWork.CoverType.Remove(id);
            unitOfWork.Save();
             return Json(new
            {
                success = true,
                message = $"{coverType.Name} is deleted."
            });
        }

        
        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = unitOfWork.CoverType.GetAll();
            return Json(new { data = allObj });
        }
        #endregion
    }
}
