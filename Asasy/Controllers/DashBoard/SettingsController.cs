using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Settings;
using Asasy.Infrastructure.Extension;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.SettingServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.Setting)]
    public class SettingsController : Controller
    {
        private readonly ISettingServices _settingServices;
        private readonly ApplicationDbContext _context;
        public SettingsController(ISettingServices settingServices, ApplicationDbContext context)
        {
            _settingServices = settingServices;
            _context = context;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var editsetting = await _settingServices.GetSetting(id);

            if (editsetting == null)
            {
                return NotFound();
            }
            return View(editsetting);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SettingEditViewModel editSettingViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _settingServices.EditSetting(editSettingViewModel);
                }
                catch (Exception ex)
                {
                    if (! _settingServices.SettingExists(editSettingViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(editSettingViewModel);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Policy(string lang = "ar")
        {
            var poilc = _context.Settings.Select(c => new PlicyViewModel
            {
                PolicyUser = lang == "ar" ? c.PolicyAr : c.PolicyEn,
            }).FirstOrDefault();

            return View(poilc);
        }

    }
}