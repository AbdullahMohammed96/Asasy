using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Cities;
using Asasy.Domain.ViewModel.Package;
using Asasy.Infrastructure.Extension;
using Asasy.Service.DashBoard.Contract.PackagesInterface;
using Microsoft.AspNetCore.Mvc;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.Packagees)]
    public class AsasyPackagesController : Controller
    {
        private readonly IPackageService _packageService;

        public AsasyPackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public async Task<IActionResult> Index()
        {
            var data =await _packageService.Packages();
            return View(data);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddNewPackageViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _packageService.AddNewPackage(model))
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var package = await _packageService.PackageDetails(Id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PackageDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _packageService.EditPackage(model))
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var check = await _packageService.Delete(id);

            return Json(new { data = check.Check, msg = check.Message });
        }

        public async Task<IActionResult> ChangeState(int id)
        {
            bool IsActive = await _packageService.ChangeState(id);
            return Json(new { key = 1, data = IsActive });
        }
    }
}
