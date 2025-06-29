using Asasy.Domain.Enums;
using Asasy.Infrastructure.Extension;
using Asasy.Service.DashBoard.Contract.RatesInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.Rates)]

    public class RatesController : Controller
    {
        private readonly IRatesService _ratesService;

        public RatesController(IRatesService ratesService)
        {
            _ratesService = ratesService;
        }

        public async Task<IActionResult> Index()
        {
            var data =await _ratesService.RatesProvider();
            return View(data);
        }
    }
}
