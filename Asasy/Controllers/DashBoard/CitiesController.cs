using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Cities;
using Asasy.Infrastructure.Extension;
using Asasy.Service.DashBoard.Contract.CitiesInterfaces;
using Asasy.Service.DashBoard.Contract.RegionsInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.Cities)]
    public class CitiesController : Controller
    {
        private readonly ICityServices _cityServices;
        private readonly IRegionServices _regionServices;

        public CitiesController(ICityServices cityServices, IRegionServices regionServices)
        {
            _cityServices = cityServices;
            _regionServices = regionServices;
        }

        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        // GET: Cities
        public async Task<IActionResult> Index()
        {
            return View(await _cityServices.GetAllCities());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Regions = _regionServices.GetRegions();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCityViewModel createCityViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _cityServices.CreateCity(createCityViewModel))
                    return RedirectToAction(nameof(Index));
            }
            return View(createCityViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            var city = await _cityServices.GetCityDetails(Id);
            if (city == null)
            {
                return NotFound();
            }
            ViewBag.Regions = _regionServices.GetRegions();

            return View(city);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCityViewModel editCityViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _cityServices.EditCity(editCityViewModel))
                    return RedirectToAction(nameof(Index));
            }
            return View(editCityViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeState(int? id)
        {
            bool IsActive = await _cityServices.ChangeState(id);
            return Json(new { data = IsActive });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var check = await _cityServices.DeleteCity(id);
            string message = check ? "تم الحذف بنجاح" : "لا يمكن حذف مدينه بها اعلانات";
            return Json(new { data = check, msg = message });
        }

    }
}
