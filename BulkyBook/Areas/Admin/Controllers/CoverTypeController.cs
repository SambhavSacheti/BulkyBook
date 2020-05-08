using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Dapper;
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
            var parameter = new DynamicParameters();
            parameter.Add("@id", id);
            coverType = unitOfWork.StoredProc_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameter);
            return coverType == null ?
                           (IActionResult)NotFound()
                         : View(coverType);
        }

        [HttpPost]
        public IActionResult Upsert(CoverType coverType)
        {
            if (!ModelState.IsValid)
                return View(coverType);
            var parameter = new DynamicParameters();
            parameter.Add("@Name", coverType.Name);
            if (coverType.Id == 0)
            {
                unitOfWork.StoredProc_Call.Execute(SD.Proc_CoverType_Create, parameter);
            }
            else
            {
                parameter.Add("@Id", coverType.Id);
                unitOfWork.StoredProc_Call.Execute(SD.Proc_CoverType_Update, parameter);
            }
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = unitOfWork.StoredProc_Call.List<CoverType>(SD.Proc_CoverType_GetAll, null);
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@id", id);
            var coverType = unitOfWork.StoredProc_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameter);
            if (coverType == null)
                return Json(new
                {
                    success = false,
                    message = "Cover Type not found"
                });
            unitOfWork.StoredProc_Call.Execute(SD.Proc_CoverType_Delete, parameter);
            return Json(new
            {
                success = true,
                message = $"{coverType.Name} is deleted."
            });
        }
        #endregion
    }
}
