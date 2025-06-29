using Asasy.Domain.Enums;
using Asasy.Infrastructure.Extension;
using Asasy.Service.DashBoard.Contract.ReportInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.Reports)]

    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> ReportsAds()
        {
            var data  = await _reportService.ReportsAds();
            return View(data);
        }

        public async Task<IActionResult> ReportsProvider()
        {
            var data =await _reportService.ReportsProvider();
            return View(data);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var check = await _reportService.DeleteReport(id);
            string message = check ? "تم الحذف بنجاح" : "حدث خطأ ما";
            return Json(new { data = check , msg = message });
        }
    }
}
