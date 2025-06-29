using Asasy.Domain.Enums;
using Asasy.Infrastructure.Extension;
using Asasy.Service.DashBoard.Contract.PaymentsInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.Payments)]

    public class PaymentsController : Controller
    {
        private readonly IPaymentsService _paymentsService;

        public PaymentsController(IPaymentsService paymentsService)
        {
            _paymentsService = paymentsService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _paymentsService.Payments();
            return View(data);
        }
    }
}
