using AAITHelper;
using AAITHelper.ModelHelper;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.Entities.Advertsments;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Ads;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.AdvertsmentInterface;
using Microsoft.EntityFrameworkCore;

namespace Asasy.Service.DashBoard.Implementation.AdvertsmentImplementation
{
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    //ssssssssssss
    public class AdsService : IAdsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHelper _uploadImage;

        public AdsService(ApplicationDbContext context, IHelper uploadImage)
        {
            _context = context;
            _uploadImage = uploadImage;
        }

        public async Task<bool> AcceptAdsToSpecial(int id)
        {
            var userPackage = _context.UserPackages.Include(a => a.Ads).Where(d => d.Id == id).FirstOrDefault();
            if (userPackage != null)
            {
                userPackage.StatusReview = (int)StatusReviewForSpecialProduct.Accept;
                userPackage.Ads.IsSpecial = true;

                NotifyUser notify = new NotifyUser
                {
                    Date = HelperDate.GetCurrentDate(),
                    TextAr = "تم قبول طلب تمييز اعلانك " + userPackage.Ads.Title + " من قبل الادمن ",
                    TextEn = "Your ad feature request has been accepted " + userPackage.Ads.Title + " has been modified by the admin",
                    Type = (int)NotifyTypes.AcceptAdsToSpecial,
                    UserId = userPackage.Ads.UserId,
                    AdsId = userPackage.AdsId,
                };

                _context.NotifyUsers.Add(notify);
                _context.SaveChanges();


                var setting = await _context.Settings.Select(x => new { x.SenderId, x.ApplicationId }).FirstOrDefaultAsync();

                var parentDeviceIds = await _context.DeviceIds.Where(x => x.UserId == notify.UserId).Select(x => new DeviceIdModel
                {
                    DeviceId = x.DeviceId_,
                    DeviceType = x.DeviceType,
                    FkUser = x.ProjectName,
                    ProjectName = x.ProjectName
                })
                .ToListAsync();

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("info", userPackage.AdsId.ToString());
                data.Add("type", notify.Type.ToString());

                var user = _context.Users.Where(d => d.Id == userPackage.Ads.UserId).FirstOrDefault();

                NotificationHelper
                    .SendPushNewNotificationAsync(parentDeviceIds, data, user.Lang == "ar" ? notify.TextAr : notify.TextEn);

                return true;
            }
            return false;
        }

        public async Task<AdEditViewModel> AdDetails(int AdsId)
        {
            var ad = _context.AdvertsmentDetails.Where(d => d.Id == AdsId).Select(c => new AdEditViewModel
            {
                AdsId = AdsId,
                CategoryId = c.CategoryId,
                CityId = c.CityId,
                Description = c.Description,
                DistrictId = c.DistrictId,
                Lat = c.Lat,
                Lng = c.Lng,
                Location = c.Location,
                Phone = c.Phone,
                PhoneCode = "+966",
                Price = c.Price,
                RegionId = c.RegionId,
                ShowPhone = c.ShowPhone,
                StatusProduct = c.StatusProduct,
                SubCategoryId = c.SubCategoryId,
                Title = c.Title,
                Images = c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).ToList(),

            }).FirstOrDefault();

            return ad;

        }

        public async Task<List<string>> AdImages(int AdsId)
        {
            var images = _context.AdvertsmentImages.Where(d => d.AdsId == AdsId).Select(c => DashBordUrl.baseUrlHost + c.Image).ToList();

            return images;
        }

        public async Task<List<AdsListViewModel>> AllAds()
        {
            var ads = _context.AdvertsmentDetails.Where(d => !d.User.IsDeleted).Select(c => new AdsListViewModel
            {
                AdsId = c.Id,
                Category = c.Category.NameAr,
                City = c.City.NameAr,
                CreationDate = HelperDate.FormatDate(c.CreationDate, "ar"),
                Description = c.Description,
                District = c.District.NameAr,
                IsActive = c.IsActive,
                IsSpecial = c.IsSpecial,
                Phone = c.Phone + " " + c.PhoneCode,
                PhoneCode = c.PhoneCode,
                Price = c.Price,
                Region = c.Region.NameAr,
                StatusProduct = c.StatusProduct == (int)StatusProduct.New ? "جديد" : "مستعمل",
                SubCategory = c.SubCategory.NameAr,
                Title = c.Title
            }).OrderByDescending(a => a.AdsId).ToList();

            return ads;
        }

        public async Task<List<PlaceListViewModel>> Categories(int AdsId)
        {
            var categories = _context.AdvertsmentDetails.Include(d => d.Category).Where(d => d.Id == AdsId).Select(c => new PlaceListViewModel
            {
                Id = c.CategoryId,
                Name = c.Category.NameAr
            }).ToList();

            return categories;
        }

        public async Task<bool> ChangeStatus(int AdsId)
        {
            var ad = _context.AdvertsmentDetails.Where(d => d.Id == AdsId).FirstOrDefault();
            ad.IsActive = !ad.IsActive;
            _context.SaveChanges();
            return ad.IsActive;
        }

        public async Task<List<PlaceListViewModel>> Cities(int AdsId)
        {
            var cities = _context.AdvertsmentDetails.Include(d => d.City).Where(d => d.Id == AdsId).Select(c => new PlaceListViewModel
            {
                Id = c.CityId,
                Name = c.City.NameAr
            }).ToList();

            return cities;
        }

        public async Task<bool> DeleteAd(int AdsId)
        {
            var ad = _context.AdvertsmentDetails.Where(d => d.Id == AdsId).FirstOrDefault();

            if (ad != null)
            {
                ad.IsDelete = true;
                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<PlaceListViewModel>> Districts(int AdsId)
        {
            var districts = _context.AdvertsmentDetails.Include(d => d.District).Where(d => d.Id == AdsId).Select(c => new PlaceListViewModel
            {
                Id = c.DistrictId,
                Name = c.District.NameAr
            }).ToList();

            return districts;
        }

        public async Task<bool> EditAds(AdEditViewModel model)
        {
            var ad = _context.AdvertsmentDetails.Where(d => d.Id == model.AdsId).FirstOrDefault();

            if (ad != null)
            {
                ad.Title = model.Title;
                ad.Description = model.Description;
                ad.Price = model.Price;
                ad.RegionId = model.RegionId;
                ad.CityId = model.CityId;
                ad.DistrictId = model.DistrictId;
                ad.CategoryId = model.CategoryId;
                ad.SubCategoryId = model.SubCategoryId;
                ad.StatusProduct = model.StatusProduct;
                ad.ShowPhone = model.ShowPhone;
                ad.Phone = model.Phone;
                ad.PhoneCode = model.PhoneCode;

                var oldImages = _context.AdvertsmentImages.Where(d => d.AdsId == ad.Id).ToList();
                _context.AdvertsmentImages.RemoveRange(oldImages);

                _context.SaveChanges();

                if (model.oldImages != null)
                {
                    var images = model.oldImages.Split(',');

                    for (int i = 0; i < images.Length; i++)
                    {
                        if (images[i] != "")
                        {
                            var image = images[i].Split(".com/");

                            AdvertsmentImages imagesEditingAds = new AdvertsmentImages
                            {
                                AdsId = model.AdsId,
                                Image = image[1],
                            };

                            _context.AdvertsmentImages.Add(imagesEditingAds);
                        }
                    }
                }

                if (model.NewImages != null)
                {
                    foreach (var image in model.NewImages)
                    {
                        AdvertsmentImages imagesEditingAds = new AdvertsmentImages
                        {
                            AdsId = model.AdsId,
                            Image = _uploadImage.Upload(image, (int)FileName.Advertisment),
                        };

                        _context.AdvertsmentImages.Add(imagesEditingAds);
                    }
                }

                NotifyUser notify = new NotifyUser
                {
                    Date = HelperDate.GetCurrentDate(),
                    TextAr = " تم تعديل اعلانك " + ad.Title + " من قبل الادمن ",
                    TextEn = "Your ad" + ad.Title + " has been modified by the admin",
                    Type = (int)NotifyTypes.EdiAdsByAdmin,
                    UserId = ad.UserId,
                    AdsId = ad.Id,
                    Show = false
                };

                _context.NotifyUsers.Add(notify);
                _context.SaveChanges();



                var parentDeviceIds = await _context.DeviceIds.Where(x => x.UserId == notify.UserId).Select(x => new DeviceIdModel
                {
                    DeviceId = x.DeviceId_,
                    DeviceType = x.DeviceType,
                    FkUser = x.ProjectName,
                    ProjectName = x.ProjectName
                })
                .ToListAsync();

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("info", ad.Id.ToString());
                data.Add("type", notify.Type.ToString());


                var user = _context.Users.Where(d => d.Id == ad.UserId).FirstOrDefault();

                NotificationHelper
                    .SendPushNewNotificationAsync(parentDeviceIds, data, user.Lang == "ar" ? notify.TextAr : notify.TextEn);

                return true;
            }
            return false;
        }

        public async Task<bool> RefuseAdsToSpecial(int id)
        {
            var userPackage = _context.UserPackages.Include(a => a.Ads).Where(d => d.Id == id).FirstOrDefault();
            if (userPackage != null)
            {
                var user = _context.Users.Where(d => d.Id == userPackage.UserId).FirstOrDefault();

                user.Wallet += userPackage.Price;
                userPackage.StatusReview = (int)StatusReviewForSpecialProduct.Refuse;



                NotifyUser notify = new NotifyUser
                {
                    Date = HelperDate.GetCurrentDate(),
                    TextAr = "تم رفض طلب تمييز اعلانك " + userPackage.Ads.Title + " من قبل الادمن ",
                    TextEn = "Your ad feature request has been rejected " + userPackage.Ads.Title + " has been modified by the admin",
                    Type = (int)NotifyTypes.RefuseAdsToSpecial,
                    UserId = userPackage.Ads.UserId,
                    AdsId = userPackage.AdsId,
                };

                _context.NotifyUsers.Add(notify);
                _context.SaveChanges();


                var setting = await _context.Settings.Select(x => new { x.SenderId, x.ApplicationId }).FirstOrDefaultAsync();

                var parentDeviceIds = await _context.DeviceIds.Where(x => x.UserId == notify.UserId).Select(x => new DeviceIdModel
                {
                    DeviceId = x.DeviceId_,
                    DeviceType = x.DeviceType,
                    FkUser = x.ProjectName,
                    ProjectName = x.ProjectName
                })
                .ToListAsync();

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("info", userPackage.AdsId.ToString());
                data.Add("type", notify.Type.ToString());


                NotificationHelper
                    .SendPushNewNotificationAsync(parentDeviceIds, data, user.Lang == "ar" ? notify.TextAr : notify.TextEn);

                return true;
            }
            return false;
        }

        public async Task<List<PlaceListViewModel>> Regions(int AdsId)
        {
            var regions = _context.AdvertsmentDetails.Include(d => d.Region).Where(d => d.Id == AdsId).Select(c => new PlaceListViewModel
            {
                Id = c.RegionId,
                Name = c.Region.NameAr
            }).ToList();

            return regions;
        }

        public async Task<List<AdsSpecialListViewModel>> RequestSpecilaAds()
        {
            var ads = _context.UserPackages.Where(d => d.StatusReview == (int)StatusReviewForSpecialProduct.Waiting).Select(c => new AdsSpecialListViewModel
            {
                Id = c.Id,
                AdsId = c.AdsId,
                AdsTitle = c.Ads.Title,
                EndDate = c.EndDate.ToString("dd/MM/yyyy"),
                IsPayed = c.IsPayed,
                PackageDescriptionAr = c.PackageDescriptionAr,
                PackageDescriptionEn = c.PackageDescriptionEn,
                PackageNameAr = c.PackageNameAr,
                PackageNameEn = c.PackageNameEn,
                Price = c.Price,
                SubscriptionDate = c.SubscriptionDate.ToString("dd/MM/yyyy"),
                User = c.User.user_Name
            }).OrderByDescending(o => o.Id).ToList();

            return ads;
        }

        public async Task<List<PlaceListViewModel>> SubCategories(int AdsId)
        {
            var subCategories = _context.AdvertsmentDetails.Include(d => d.SubCategory).Where(d => d.Id == AdsId).Select(c => new PlaceListViewModel
            {
                Id = c.SubCategoryId,
                Name = c.SubCategory.NameAr
            }).ToList();

            return subCategories;
        }
    }
}
