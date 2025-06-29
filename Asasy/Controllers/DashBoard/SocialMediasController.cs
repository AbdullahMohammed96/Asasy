using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.SocialMedia;
using Asasy.Infrastructure.Extension;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.SocialMediaInterfases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.SocialMedia)]
    public class SocialMediasController : Controller
    {
        private readonly ISocialMediaServices _socialMediaServices;

        public SocialMediasController(ISocialMediaServices socialMediaServices)
        {
            _socialMediaServices = socialMediaServices;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _socialMediaServices.GetSocialMedia());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SocialMediaAddViewModel socialMedia)
        {
            if (await _socialMediaServices.CreateSocialMedia(socialMedia))
                return RedirectToAction(nameof(Index));

            return View(socialMedia);
        }

        //GET: DSocialMedias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(await _socialMediaServices.GetSocialMediaDetails(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SocialMediaEditViewModel editSocialMediaViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _socialMediaServices.EditSocialMediaDetails(editSocialMediaViewModel))
                    return RedirectToAction(nameof(Index));
            }
            return View(editSocialMediaViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeState(int? id)
        {
            bool IsActive = await _socialMediaServices.ChangeState(id);
            return Json(new { key = 1, data = IsActive });
        }
    }
}
