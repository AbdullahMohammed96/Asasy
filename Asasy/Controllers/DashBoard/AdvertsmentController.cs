using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Ads;
using Asasy.Infrastructure.Extension;
using Asasy.Service.DashBoard.Contract.AdvertsmentInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.Advertsment)]
    public class AdvertsmentController : Controller
    {
        private readonly IAdsService _adsService;

        public AdvertsmentController(IAdsService adsService)
        {
            _adsService = adsService;
        }

        public async Task<IActionResult> Index(string msg = null)
        {
            var data = await _adsService.AllAds();
            ViewBag.Msg = msg;
            return View(data);
        }


        public async Task<IActionResult> RequestSpecilaAds()
        {
            var data = await _adsService.RequestSpecilaAds();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptAdsToSpecial(int id)
        {
            try
            {
                var result = await _adsService.AcceptAdsToSpecial(id);
                return Json(new { data = result });
            }
            catch (Exception)
            {

                return Json(new { data = false });
            }

        }

        [HttpPost]
        public async Task<IActionResult> RefuseAdsToSpecial(int id)
        {
            try
            {
                var result = await _adsService.RefuseAdsToSpecial(id);
                return Json(new { data = result });
            }
            catch (Exception)
            {

                return Json(new { data = false });
            }

        }
        public async Task<IActionResult> EditAd(int adId)
        
        {
            var data = await _adsService.AdDetails(adId);

            var regions = await _adsService.Regions(adId);
            var cities = await _adsService.Cities(adId);
            var districts = await _adsService.Districts(adId);
            var categories = await _adsService.Categories(adId);
            var subCategories = await _adsService.SubCategories(adId);

            ViewBag.Regions = new SelectList(regions, "Id", "Name");
            ViewBag.Cities = new SelectList(cities, "Id", "Name");
            ViewBag.Districts = new SelectList(districts, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.SubCategories = new SelectList(subCategories, "Id", "Name");

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> EditAd(AdEditViewModel model)
       {

            if (ModelState.IsValid)
            {
                var check = await _adsService.EditAds(model);
                if (check)
                {
                    return RedirectToAction(nameof(Index), new { msg = "تم تعديل الاعلان بنجاح" });
                }
                else
                {
                    return View(model);

                }
            }

            return View(model);
        }
        public async Task<IActionResult> ChangeState(int id)
        {
            bool IsActive = await _adsService.ChangeStatus(id);
            return Json(new { key = 1, data = IsActive });
        }

        [HttpPost]
        public async Task<IActionResult> AdImages(int adsId)
        {
            var result = await _adsService.AdImages(adsId);
            return Json(new { key = 1, data = result });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAds(int adsId)
        {
            var check = await _adsService.DeleteAd(adsId);
            return Json(new { data = check });
        }

    }
}
