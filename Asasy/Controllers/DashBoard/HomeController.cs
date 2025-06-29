using AAITHelper.Enums;
using Asasy.Domain.Enums;
using Asasy.Domain.Model;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.HomeInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Asasy.Controllers.DashBoard
{
    [Authorize]

    public class HomeController : Controller
    {
        private readonly IHomeServices _homeServices;

        public HomeController(IHomeServices homeServices = null)
        {
            _homeServices = homeServices;
        }

        public IActionResult Index()
        {
            var data =_homeServices.HomeIndex();

            return View(data);
        }
    }
}
