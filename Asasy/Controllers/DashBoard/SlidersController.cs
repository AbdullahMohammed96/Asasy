using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Slider;
using Asasy.Infrastructure.Extension;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.SliderInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.Slider)]
    public class SlidersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHelper _uploadImage;
        private readonly ISliderServices _sliderServices;

        public SlidersController(ApplicationDbContext context, IHelper uploadImage, ISliderServices sliderServices)
        {
            _context = context;
            _uploadImage = uploadImage;
            _sliderServices = sliderServices;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _sliderServices.GetAllSliders());
        }

        // GET: Slider/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSliderViewModel slider)
        {
            if (ModelState.IsValid)
            {

                string[] validImages = { "jpg", "png", "jpeg", "JPG", "PNG", "JPEG" };

                var extenstionFlag = slider.Image.FileName.Split(".")[1];


                if (!validImages.Contains(extenstionFlag))
                {

                    ModelState.AddModelError("", "فضلا قم بتحميل صور فقط");
                    return View(slider);

                }

                if (await _sliderServices.CreateSlider(slider))
                    return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Slider = await _sliderServices.GetSliderDetails(id);
            if (Slider == null)
            {
                return NotFound();
            }

            return View(Slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditSliderViewModel slider)
        {
            if (ModelState.IsValid)
            {
                if(slider.NewImage != null)
                {
                    string[] validImages = { "jpg", "png", "jpeg", "JPG", "PNG", "JPEG" };

                    var extenstionFlag = slider.NewImage.FileName.Split(".")[1];


                    if (!validImages.Contains(extenstionFlag))
                    {

                        ModelState.AddModelError("", "فضلا قم بتحميل صور فقط");
                        return View(slider);

                    }
                }
                if (await _sliderServices.EditSlider(slider))
                    return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeState(int? id)
        {
            bool IsActive = await _sliderServices.ChangeState(id);

            return Json(new { key = 1, data = IsActive });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            bool IsDelete = await _sliderServices.Delete(id);

            return Json(new { key = 1, data = IsDelete });
        }
    }
}
