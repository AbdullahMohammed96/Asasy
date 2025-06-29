using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Cities;
using Asasy.Domain.ViewModel.ProhibitedGoods;
using Asasy.Infrastructure.Extension;
using Asasy.Service.DashBoard.Contract.ProhibitedGoodsInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.ProhibitedGoods)]
    public class ProhibitedGoodsController : Controller
    {
        private readonly IProhibitedGoodsService _prohibitedGoodsService;

        public ProhibitedGoodsController(IProhibitedGoodsService prohibitedGoodsService)
        {
            _prohibitedGoodsService = prohibitedGoodsService;
        }

        public async Task<IActionResult> Index()
        {
            var data =await _prohibitedGoodsService.ProhibitedGoods();
            return View(data);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(NewProhibitedGoodViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _prohibitedGoodsService.CreateProhibitedGood(model))
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var data = await _prohibitedGoodsService.ProhibitedInfo(Id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditProhibitedGoodsViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _prohibitedGoodsService.EditProhibitedGood(model))
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var check = await _prohibitedGoodsService.DeleteProhibitedGoods(id);
            string message = check ? "تم الحذف بنجاح" : "حدث خطأ ما";

            return Json(new { data = check, msg = message });
        }

        public async Task<IActionResult> ChangeState(int id)
        {
            bool IsActive = await _prohibitedGoodsService.ChangeStatus(id);
            return Json(new { key = 1, data = IsActive });
        }
    }
}
