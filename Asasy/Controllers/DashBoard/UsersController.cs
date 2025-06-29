using Asasy.Domain.Common.Helpers.DataTablePaginationServer;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.DTO.UserIndexDto;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Asasy.Infrastructure.Extension;
using Asasy.Persistence;
using Microsoft.AspNetCore.Mvc;
using Asasy.Service.DashBoard.Contract.UserInterfaces;
using Asasy.Domain.ViewModel.Users;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.Clients)]

    public class UsersController : Controller
    {
        private readonly IUserServices _userServices;

        public UsersController(IHelper helper, IAppService appService, IUserServices userServices)
        {
            _userServices = userServices;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _userServices.GetUsers());
        }

        public async Task<IActionResult> Create()
        {

           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                string[] validImages = { "jpg", "png", "jpeg", "JPG", "PNG", "JPEG" };

                if (model.Image != null)
                {

                    var extenstionImage = model.Image.FileName.Split(".")[1];

                    if (!validImages.Contains(extenstionImage))
                    {

                        ModelState.AddModelError("", "فضلا قم بتحميل صور فقط");
                        return View(model);

                    }
                }
                var check = await _userServices.CreateUser(model);
                if (check.Check)
                {
                    return RedirectToAction(nameof(Index), new { msg = "تم اضافه العميل بنجاح" });
                }



                ModelState.AddModelError("", check.Message);
                return View(model);
            }

            return View(model);


        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var check = await _userServices.Delete(id);

            return Json(new { data = check.Check, msg = check.Message });
        }

        public async Task<IActionResult> ChangeState(string id)
        {
            bool IsActive = await _userServices.ChangeState(id);
            return Json(new { key = 1, data = IsActive });
        }
    }
}
