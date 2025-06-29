using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Cities;
using Asasy.Domain.ViewModel.Districts;
using Asasy.Infrastructure.Extension;
using Asasy.Service.DashBoard.Contract.DistrictInterface;
using Microsoft.AspNetCore.Mvc;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.District)]
    public class DistrictController : Controller
    {
        private readonly IDistrictService _districtService;

        public DistrictController(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _districtService.Districts();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Cities = _districtService.GetCities().Result;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDistrictViewModel createCityViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _districtService.CreateDistrict(createCityViewModel))
                    return RedirectToAction(nameof(Index));
            }
            ViewBag.Cities = _districtService.GetCities().Result;

            return View(createCityViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var city = await _districtService.GetDistrictDetails(Id);
            if (city == null)
            {
                return NotFound();
            }
            ViewBag.Cities = _districtService.GetCities().Result;

            return View(city);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditDistrictViewModel editDistrictViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _districtService.EditDistrict(editDistrictViewModel))
                    return RedirectToAction(nameof(Index));
            }
            ViewBag.Cities = _districtService.GetCities().Result;

            return View(editDistrictViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeState(int id)
        {
            bool IsActive = await _districtService.ChangeState(id);
            return Json(new { data = IsActive });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var check = await _districtService.DeleteDistrict(id);
            string message = check ? "تم الحذف بنجاح" : "لا يمكن حذف حي به اعلانات";
            return Json(new { data = check, msg = message });
        }
    }
}
