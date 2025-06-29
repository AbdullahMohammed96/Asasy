using AAITHelper;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.Entities.Categories;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.CategoryModel;
using Asasy.Infrastructure.Extension;
using Asasy.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRolesAttribute(Roles.Admin, Roles.Category)]

    public class CategoryController : Controller
    {
        private readonly UserManager<ApplicationDbUser> _userManager;

        private readonly ApplicationDbContext _context;
        private readonly IHelper _uploadImg;

        public CategoryController(ApplicationDbContext _db,
            UserManager<ApplicationDbUser> userManager, IHelper uploadImg)
        {
            _userManager = userManager;
            _uploadImg = uploadImg;
            _context = _db;
        }



        public IActionResult MainCategory(string msg)
        {
            var UserId = _userManager.GetUserId(User);

            List<MainCategoryViewModel> MainCategory = _context.Categories
                .Select(x => new MainCategoryViewModel
                {
                    Id = x.Id,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    IsActive = x.IsActive,
                    Img = DashBordUrl.baseUrlHost + x.Image
                }).OrderByDescending(x => x.Id).ToList();
            if (msg != null)
            {
                ViewBag.Msg = msg;
            }
            return View(MainCategory);
        }

        [HttpGet]
        public IActionResult CreateMainCategory(string msg, int idCat = 0, string CatName = "")
        {
            if (idCat != 0)
            {
                ViewBag.idCat = idCat;
                ViewBag.CatName = CatName;
            }
            else
            {
                ViewBag.idCat = 0;
            }

            if (msg != null)
            {
                ViewBag.Msg = msg;
            }

            return View();
        }

        [HttpPost]
        public IActionResult CreateMainCategory(MainCategoryViewModel model)
        {
            try
            {
                var UserId = _userManager.GetUserId(User);


                int CountCategory = _context.Categories.Where(x => x.IsActive).Count();
                if (model.idCat == 0)
                {
                    if (model.CreateImg == null || string.IsNullOrEmpty(model.NameAr) || string.IsNullOrEmpty(model.NameEn))
                    {
                        ModelState.AddModelError("NameAr", "يرجى التاكد من البيانات");
                        return View(model);
                    }
                    string[] validImages = { "jpg", "png", "jpeg", "JPG", "PNG", "JPEG" };

                    var extenstionFlag = model.CreateImg.FileName.Split(".")[1];


                    if (!validImages.Contains(extenstionFlag))
                    {

                        ModelState.AddModelError("", "فضلا قم بتحميل صور فقط");
                        return View(model);

                    }

                }
                else
                {
                    if (model.CreateImg == null || string.IsNullOrEmpty(model.NameAr) || string.IsNullOrEmpty(model.NameEn))
                    {
                        ModelState.AddModelError("NameAr", "يرجى التاكد من البيانات");
                        return View();
                    }


                    string[] validImages = { "jpg", "png", "jpeg", "JPG", "PNG", "JPEG" };

                    var extenstionFlag = model.CreateImg.FileName.Split(".")[1];


                    if (!validImages.Contains(extenstionFlag))
                    {

                        ModelState.AddModelError("", "فضلا قم بتحميل صور فقط");
                        return View(model);

                    }

                    SubCategories subCategory = new SubCategories
                    {
                        CategoryId = (int)model.idCat,
                        Date = HelperDate.GetCurrentDate(),
                        IsDeleted = false,
                        Image = _uploadImg.Upload(model.CreateImg, (int)FileName.SubCategory),
                        IsActive = true,
                        NameAr = model.NameAr,
                        NameEn = model.NameEn,
                    };

                    _context.SubCategories.Add(subCategory);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(SubCategory), new { id = model.idCat, name = model.CatName, msg = "تم االاضافه بنجاح" });

                }


                Category category = new Category
                {

                    NameAr = model.NameAr,
                    NameEn = model.NameEn,
                    IsActive = true,
                    Image = _uploadImg.Upload(model.CreateImg, (int)FileName.Category),
                    Date = DateTime.Now,
                    IsDeleted = false,

                };
                _context.Categories.Add(category);
                _context.SaveChanges();

                if (model.idCat == 0)
                {
                    return RedirectToAction(nameof(MainCategory), new { msg = "تم االاضافه بنجاح" });
                }
                else
                {
                    return RedirectToAction(nameof(SubCategory), new { id = model.idCat, name = model.CatName, msg = "تم االاضافه بنجاح" });
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("NameAr", ex.Message);
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditMainCategory(string msg, int? id, string type = "", int parentId = 0, string catParentName = "")
        {
            var UserId = _userManager.GetUserId(User);

            if (id == null)
            {
                return NotFound();
            }

            //var category =  _context.Categories.Where(x => x.Id == id).FirstOrDefault();


            dynamic category;
            if (type == "Sub")
            {
                category = await _context.SubCategories.Where(x => x.Id == id).FirstOrDefaultAsync();
            }
            else
            {
                category = await _context.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();

            }
            int CountCategory = await _context.Categories.Where(x => x.IsActive == true).CountAsync();

            if (category == null)
            {
                return NotFound();
            }

            ViewBag.TypeCat = type;
            ViewBag.CatName = catParentName;
            ViewBag.ParentId = parentId;

            MainCategoryViewModel model = new MainCategoryViewModel()
            {
                Id = category.Id,
                NameAr = category.NameAr,
                NameEn = category.NameEn,
                Img = DashBordUrl.baseUrlHost + category.Image,
                CountCategory = CountCategory,
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditMainCategory(MainCategoryViewModel model)
        {
            var UserId = _userManager.GetUserId(User);


            dynamic category;
            if (model.type == "Sub")
            {
                category = await _context.SubCategories.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            }
            else
            {
                category = await _context.Categories.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            }

            string NewString = default;

            if (model.CreateImg != null)
            {

                string[] validImages = { "jpg", "png", "jpeg", "JPG", "PNG", "JPEG" };

                var extenstionFlag = model.CreateImg.FileName.Split(".")[1];


                if (!validImages.Contains(extenstionFlag))
                {

                    ModelState.AddModelError("", "فضلا قم بتحميل صور فقط");
                    return View(model);

                }


                NewString = _uploadImg.Upload(model.CreateImg, (int)FileName.Category);
            }


            if (string.IsNullOrEmpty(model.NameAr) || string.IsNullOrEmpty(model.NameEn))
            {
                ModelState.AddModelError("NameAr", "يرجى التاكد من البيانات");
                return View();
            }

            category.NameAr = model.NameAr;
            category.NameEn = model.NameEn;
            category.Image = NewString == null ? category.Image : NewString;
            //category.IsActive = true;

            _context.SaveChanges();

            //MainCategoryViewModel model = new MainCategoryViewModel()
            //{
            //    Id = category.Id,
            //    NameAr = category.NameAr,
            //    NameEn = category.NameEn,
            //    Img = category.Img
            //};

            if (model.type == "Main")
                return RedirectToAction("MainCategory", new { msg = "تم التعديل بنجاح" });

            return RedirectToAction("SubCategory", new { id = model.idCat, name = model.CatName, msg = "تم التعديل بنجاح" });
        }


        public IActionResult SubCategory(string msg, int id, string name)
        {
            var UserId = _userManager.GetUserId(User);

            List<MainCategoryViewModel> MainCategory = _context.SubCategories.Where(d => d.CategoryId == id)
                .Select(x => new MainCategoryViewModel
                {
                    Id = x.Id,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    IsActive = x.IsActive,
                    Img = DashBordUrl.baseUrlHost + x.Image
                }).ToList();

            ViewBag.CatName = name;
            ViewBag.IdCat = id;

            if (msg != null)
            {
                ViewBag.Msg = msg;
            }
            return View(MainCategory);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var foundedCategory = _context.Categories.Find(id);



            if (foundedCategory != null)
            {
                var isContainAds = _context.AdvertsmentDetails.Where(d => d.CategoryId == id && !d.IsDelete).Any();

                if (isContainAds)
                {
                    return Json(new { key = -1 });
                }
                foundedCategory.IsDeleted = true;
            }

            await _context.SaveChangesAsync();

            return Json(new { key = 1 });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteSubCategory(int id)
        {
            var foundedCategory = _context.SubCategories.Find(id);
            if (foundedCategory != null)
            {
                var isContainAds = _context.AdvertsmentDetails.Where(d => d.SubCategoryId == id && !d.IsDelete).Any();
                if (isContainAds)
                {
                    return Json(new { key = -1 });
                }
                foundedCategory.IsDeleted = true;
            }

            await _context.SaveChangesAsync();

            return Json(new { key = 1 });
        }

        [HttpPost]
        public async Task<bool> ChangeStatus(int id)
        {
            var Data = await _context.Categories.FindAsync(id);

            Data.IsActive = !Data.IsActive;

            await _context.SaveChangesAsync();


            return Data.IsActive;
        }

        [HttpPost]
        public async Task<bool> ChangeStatusForSubCategory(int id)
        {
            var Data = await _context.SubCategories.FindAsync(id);

            Data.IsActive = !Data.IsActive;

            await _context.SaveChangesAsync();


            return Data.IsActive;
        }


    }
}
