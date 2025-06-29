using AAITHelper;
using AAITHelper.Enums;
using AAITHelper.ModelHelper;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Notification;
using Asasy.Domain.ViewModel.Settings;
using Asasy.Infrastructure.Extension;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Auth;
using Asasy.Service.DashBoard.Contract.NotificationInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.Notifications)]
    public class SendNotificationController : Controller
    {
        private readonly INotificationServices _notificationServices;

        public SendNotificationController(INotificationServices notificationServices = null)
        {
            _notificationServices = notificationServices;
        }

        public async Task<IActionResult> Index()
        {
            var UserNotifies = await _notificationServices.GetHistoryNotify();

            return View(UserNotifies);

        }

        public IActionResult SendNotify()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _notificationServices.GetUsers();

            return Ok(new { key = 1, users });
        }

        [HttpGet]
        public async Task<IActionResult> GetDeleget()
        {
            var delegets = await _notificationServices.GetDeleget();

            return Ok(new { key = 1, delegets });
        }

        [HttpPost]
        public async Task<IActionResult> Send(string msg, string employees, string providers)
        {

            await _notificationServices.Send(msg, employees, providers);

            return Ok(new { redirectToUrl = Url.Action("Index", "SendNotification") });
        }
    }
}