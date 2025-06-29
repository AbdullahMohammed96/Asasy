using Asasy.Service.Api.Contract.Auth;
using Asasy.Service.DashBoard.Contract.ComplaintsInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Asasy.Controllers.DashBoard
{
    public class ComplaintsController : Controller
    {
        private readonly IComplaintsService _complaintsService;
        private readonly ICurrentUserService _currentUserService;

        public ComplaintsController(IComplaintsService complaintsService, ICurrentUserService currentUserService)
        {
            _complaintsService = complaintsService;
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> Index()
        {
            var data =await _complaintsService.Complaints();
            return View(data);
        }


        public async Task<IActionResult> ReplayToComplaints(int id, string replay)
        {
            var userName = User.Identity.Name;

            bool check = await _complaintsService.ReplayToComplaints(id,replay, userName);
            return Json(new { key = 1, data = check });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var check = await _complaintsService.DeleteComplaints(id);
            string message = check ? "تم الحذف بنجاح" : "حدث خطأ ما";
            return Json(new { data = check,msg = message });
        }
    }
}
