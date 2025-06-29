using Asasy.Domain.Entities.Copon;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Copon;
using Asasy.Infrastructure.Extension;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.CoponsInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin)]
    public class CoponsController : Controller
    {
        private readonly ICoponServices _coponServices;

        public CoponsController(ICoponServices coponServices)
        {
            _coponServices = coponServices;
        }

        public async Task<IActionResult> Index()
        {
            var copons =await _coponServices.GetCopons();
            return View(copons);
        }

        // GET: Copons/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoponCreateViewModel createCoponViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_coponServices.IsExist(createCoponViewModel.CoponCode))
                {
                    ModelState.AddModelError("CoponCode", "هذا الكود موجود من قبل");
                    return View(createCoponViewModel);
                }

                bool IsAdded=await _coponServices.CreateCopon(createCoponViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(createCoponViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var copon = await _coponServices.GetCopon(id);
            if (copon == null)
            {
                return NotFound();
            }

            CoponCreateViewModel createCoponViewModel = new CoponCreateViewModel()
            {
                Count = copon.Count,
                Id = copon.Id,
                //CountUsed = copon.CountUsed,
                Discount = copon.Discount,
                CoponCode = copon.CoponCode,
                IsActive = copon.IsActive,
                expirdate = copon.Expirdate,
                limt_discount = copon.limtDiscount
            };
            return View(createCoponViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CoponCreateViewModel createCoponViewModel)
        {
            if (id != createCoponViewModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _coponServices.EditCopon(id, createCoponViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_coponServices.IsExist(createCoponViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(createCoponViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeState(int? id)
        {
            bool IsActive=await _coponServices.ChangeState(id);

            return Ok(new { key = 1, data = IsActive });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            bool IsDeleted=await _coponServices.DeleteCopons(id);
            return Json(new { key = 1, data = IsDeleted });
        }

    }
}