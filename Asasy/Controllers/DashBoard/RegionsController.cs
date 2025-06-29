using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Regions;
using Asasy.Infrastructure.Extension;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.RegionsInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.Regoins)]

    public class RegionsController : Controller
    {
        private readonly IRegionServices _regionServices;

        public RegionsController(IRegionServices regionServices)
        {
            _regionServices = regionServices;
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            return View(await _regionServices.GetAllRegions());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.city = _regionServices.GetAllCities(); 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRegionViewModel createRegionViewModel)
        {
            if (ModelState.IsValid)
            {
                if(await _regionServices.CreateRegion(createRegionViewModel))
                    return RedirectToAction(nameof(Index));
            }
            return View(createRegionViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            var regoin = await _regionServices.GetRegionDetails(Id);
            if (regoin == null)
            {
                return NotFound();
            }

            return View(regoin);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditRegionViewModel editRegionViewModel)
        {
            if (ModelState.IsValid)
            {
                if(await _regionServices.EditRegion(editRegionViewModel))
                    return RedirectToAction(nameof(Index));
            }

            return View(editRegionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeState(int? id)
        {
            bool IsActive=await _regionServices.ChangeState(id);
            return Json(new { data = IsActive });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var check = await _regionServices.DeleteRegion(id);
            string message = check ? "تم الحذف بنجاح" : "لا يمكن حذف منطقه بها اعلانات";
            return Json(new { data = check,msg= message });
        }
    }
}
