using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Entities.AdditionalTables;
using Asasy.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Asasy.Domain.Common.PathUrl.DashBordUrl;

namespace Asasy.Controllers.Shared
{
    public class GeneratePdfWithQrCodeController : Controller
    {
        private readonly IHelper _helper;
        private readonly ApplicationDbContext _db;

        public GeneratePdfWithQrCodeController(ApplicationDbContext db, IHelper helper)
        {
            _helper = helper;
            _db = db;
        }

        public IActionResult Index(int id)
        {
            Order order = _db.Orders.Include(x => x.OrderInfos).Where(o => o.Id == id).FirstOrDefault();
            string Qrtext = $"{baseUrlHost}/pdf/{id}.pdf";
            ViewBag.ReturnQrImg = Helper.GenerateQrcode(Qrtext);
            return View(order);
        }

        public IActionResult CreatePdf(int id)
        {
            _helper.CreatePDF("GeneratePdfWithQrCode/Index", id);
            var myfile = System.IO.File.ReadAllBytes($"wwwroot/pdf/{id}.pdf");
            return File(myfile, System.Net.Mime.MediaTypeNames.Application.Pdf);
        }
    }
}
