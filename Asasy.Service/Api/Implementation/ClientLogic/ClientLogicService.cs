using System.Globalization;
using AAITHelper;
using AAITHelper.ModelHelper;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.DTO;
using Asasy.Domain.DTO.ClientLogicDto;
using Asasy.Domain.Entities.Advertsments;
using Asasy.Domain.Entities.AsasyPackages;
using Asasy.Domain.Entities.Follow;
using Asasy.Domain.Entities.Reports;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Auth;
using Asasy.Service.Api.Contract.ClientLogic;
using Asasy.Service.Api.Contract.Lang;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;

namespace Asasy.Service.Api.Implementation.ClientLogic
{
    public class ClientLogicService : IClientLogicService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILangService _langService;
        private readonly IHelper _uploadImage;
        private readonly ICurrentUserService _currentUserService;

        public ClientLogicService(ApplicationDbContext context, ILangService langService, IHelper uploadImage, ICurrentUserService currentUserService)
        {
            _context = context;
            _langService = langService;
            _uploadImage = uploadImage;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponseDto<string>> AddNewAds(AddNewAdsDto model)
        {

            var lang = _langService.Lang;

            if (model.images != null && model.images.Count > 6)
            {
                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "عفوا لا يمكنك اضافه اكثر من 6 صور" : "Sorry, you cannot add more than 6 photos."
                };
            }

            AdvertsmentDetails ads = new AdvertsmentDetails
            {
                CategoryId = model.categoryId,
                CityId = model.cityId,
                DistrictId = model.districtId,
                Description = model.description,
                CreationDate = HelperDate.GetCurrentDate(),
                IsActive = true,
                Lat = model.lat,
                Lng = model.lng,
                Location = model.location,
                Phone = model.phone,
                PhoneCode = model.phoneCode,
                UserId = _currentUserService.UserId,
                Title = model.title,
                SubCategoryId = model.subCategoryId,
                StatusProduct = model.statusProduct,
                Price = model.price,
                RegionId = model.regionId,
                ShowPhone = model.showPhone
            };

            _context.AdvertsmentDetails.Add(ads);
            _context.SaveChanges();

            if (model.images != null)
            {
                foreach (var item in model.images)
                {
                    AdvertsmentImages advertsmentImages = new AdvertsmentImages
                    {
                        AdsId = ads.Id,
                        Image = _uploadImage.Upload(item, (int)FileName.Advertisment)
                    };
                    _context.AdvertsmentImages.Add(advertsmentImages);
                }
                //_context.SaveChanges();
            }

            var provider = _context.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();
            var followsProvider = _context.Follows.Where(d => d.Type == (int)FollowType.Provider && d.ProviderId == _currentUserService.UserId).ToList();

            foreach (var item in followsProvider)
            {
                NotifyUser notify = new NotifyUser
                {
                    UserId = item.UserId,
                    AdsId = ads.Id,
                    Date = HelperDate.GetCurrentDate(),
                    Show = false,
                    TextAr = " قام المعلن " + provider.user_Name + " باضافه الاعلان " + ads.Title,
                    TextEn = " Advertiser " + provider.user_Name + " added the ad " + ads.Title,
                    Type = (int)NotifyTypes.AddNewAds
                };

                _context.NotifyUsers.Add(notify);


                var parentDeviceIds = await _context.DeviceIds.Where(x => x.UserId == notify.UserId).Select(x => new DeviceIdModel
                {
                    DeviceId = x.DeviceId_,
                    DeviceType = x.DeviceType,
                    FkUser = x.ProjectName,
                    ProjectName = x.ProjectName
                })
                .ToListAsync();

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("info", ads.Id.ToString());
                data.Add("type", notify.Type.ToString());


                NotificationHelper
                    .SendPushNewNotificationAsync(parentDeviceIds, data, HelperMsg.creatMessage(provider.Lang, notify.TextAr, notify.TextEn));


            }

            _context.SaveChanges();



            return new BaseResponseDto<string>
            {
                IsSuccess = true,
                Message = lang == "ar" ? "تم اضافه الاعلان بنجاح" : "The ad has been added successfully.",
                Data = ads.Id.ToString()
            };

        }

        public async Task<BaseResponseDto<AdsDetailsDto>> AdsDetails(int adId, int PageSize = 10, int PageNumber = 1)
        {

            try
            {

           
            var lang = _langService.Lang;
            int maxRows = PageSize;
              

            var ads = _context.AdvertsmentDetails.Where(d => d.Id == adId).Select(c => new AdsDetailsDto
            {
                adsId = c.Id,
                providerId = c.UserId,
                category = lang == "ar" ? c.Category.NameAr : c.Category.NameEn,
                categoryId = c.CategoryId,
                sumRate = _context.UserRates.Where(d => d.ProviderId == c.UserId).Select(r => r.Rate).Sum(),
                countRate = _context.UserRates.Where(d => d.ProviderId == c.UserId).Select(r => r.Rate).Count(),
                date = HelperDate.FormatDate(c.CreationDate, lang),
                description = c.Description,
                images = c.AdvertsmentImages.Where(d => d.AdsId == adId).Select(c => DashBordUrl.baseUrlHost + c.Image).ToList(),
                isFavorite = c.Favorites.Where(d => d.AdsId == adId && d.UserId == _currentUserService.UserId).Any(),
                isFollow = c.Follows.Where(d => d.UserId == _currentUserService.UserId && d.AdsId == adId && d.Type == (int)FollowType.Ads).Any(),
                isSpecial = c.IsSpecial,
                lat = c.Lat,
                lng = c.Lng,
                location = c.Location,
                phoneNumber = c.Phone,
                statusProduct = GetProductStatus(c.StatusProduct, lang),
                price = c.Price,
                subCategory = lang == "ar" ? c.SubCategory.NameAr : c.SubCategory.NameEn,
                subCategoryId = c.SubCategoryId,
                title = c.Title,
                userImage = DashBordUrl.baseUrlHost + c.User.ImgProfile,
                userName = c.User.user_Name,
                showPhone = c.ShowPhone,
                comments = c.CommentsAds.Where(d => d.AdsId == adId).Select(n => new AdsCommentDto
                {
                    commentId = n.Id,
                    comment = n.Comment,
                    userName = n.User.user_Name,
                    date = HelperDate.FormatDate(n.CreationDate, lang),
                    userImage = DashBordUrl.baseUrlHost + n.User.ImgProfile,
                    replaies = _context.ReplaiesComments.Where(d => d.CommentId == n.Id).Select(a => new ReplaiesCommentDto
                    {
                        commentId = a.Id,
                        comment = a.Replay,
                        userName = a.User.user_Name,
                        date = HelperDate.FormatDate(a.CreationDate, lang),
                        userImage = DashBordUrl.baseUrlHost + a.User.ImgProfile,
                    }).OrderByDescending(o => o.commentId).ToList()
                }).OrderByDescending(o => o.commentId).Skip((PageNumber - 1) * maxRows).Take(maxRows).ToList(),
                similarAds = _context.AdvertsmentDetails.Where(d => d.Id != adId && d.SubCategoryId == c.SubCategoryId).Select(n => new AdsListDto
                {
                    adsId = n.Id,
                    cityName = lang == "ar" ? n.City.NameAr : n.City.NameEn,
                    date = HelperDate.FormatDate(n.CreationDate, lang),
                    districtName = lang == "ar" ? c.District.NameAr : c.District.NameEn,
                    description = c.Description,
                    image = n.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? n.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).FirstOrDefault() : null,
                    images = n.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? n.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).ToList() : null,
                    isFavorite = n.Favorites.Where(d => d.UserId == _currentUserService.UserId && d.AdsId == n.Id).Any(),
                    isSpecial = n.IsSpecial,
                    lat = n.Lat,
                    lng = n.Lng,
                    location = n.Location,
                    title = n.Title,
                    price = n.Price,
                    providerName = n.User.user_Name,
                }).OrderByDescending(h => h.adsId).Take(10).ToList(),
                chatId = _context.Chats.Where(d => d.AdsId == adId && ((d.UserId == _currentUserService.UserId && d.ProviderId == c.UserId) || (d.UserId == c.UserId && d.ProviderId == _currentUserService.UserId))).Select(n => n.Id).FirstOrDefault(),
                createAt = HelperDate.FormatDate(c.CreationDate, lang)
            }).FirstOrDefault();


                var D = 0; 

            return new BaseResponseDto<AdsDetailsDto>
            {
                Data = ads,
                IsSuccess = true,
                PaginatioData = new PaginationDto
                {
                    TotalItems = _context.CommentAds.Where(d => d.AdsId == adId).Count()
                }
            };
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        private static string GetProductStatus(int status, string lang)
        {
            string result = "";

            if (status == (int)StatusProduct.New)
            {
                result = lang == "ar" ? "جديد" : "New";
            }
            else
            {
                result = lang == "ar" ? "مستعمل" : "Old";

            }
            return result;
        }
        public async Task<BaseResponseDto<List<AdsListDto>>> Home(int PageSize = 10, int PageNumber = 1)
        {
            var lang = _langService.Lang;

            int maxRows = PageSize;

            //var ads = _context.AdvertsmentDetails.Where(d => d.UserId != _currentUserService.UserId && d.IsActive).Select(c => new AdsListDto
            var ads = _context.AdvertsmentDetails.Where(d => d.IsActive).Select(c => new AdsListDto
            {
                adsId = c.Id,
                cityName = lang == "ar" ? c.City.NameAr : c.City.NameEn,
                description = c.Description,
                date = HelperDate.FormatDate(c.CreationDate, lang),
                districtName = lang == "ar" ? c.District.NameAr : c.District.NameEn,
                image = c.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).FirstOrDefault() : null,
                images = c.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).ToList() : null,
                isFavorite = c.Favorites.Where(d => d.UserId == _currentUserService.UserId && d.AdsId == c.Id).Any(),
                isSpecial = c.IsSpecial,
                lat = c.Lat,
                lng = c.Lng,
                location = c.Location,
                title = c.Title,
                price = c.Price,
                providerName = c.User.user_Name,

            }).OrderByDescending(a => a.adsId).Skip((PageNumber - 1) * maxRows).Take(maxRows).ToList();




            return new BaseResponseDto<List<AdsListDto>>
            {
                Data = ads,
                IsSuccess = true,
                PaginatioData = new PaginationDto
                {
                    TotalItems = _context.AdvertsmentDetails.Where(d => d.UserId != _currentUserService.UserId && d.IsActive).Count()
                }
            };

        }

        public async Task<BaseResponseDto<List<AdsListDto>>> AdsByLocation(string lat, string lng, int PageSize = 10, int PageNumber = 1)
        {
            var lang = _langService.Lang;

            int maxRows = PageSize;
            //var ads = _context.AdvertsmentDetails.Where(d => d.IsActive && d.UserId != _currentUserService.UserId)
            var ads = _context.AdvertsmentDetails.Where(d => d.IsActive)
                .Include(x => x.User)
                .Include(x => x.AdvertsmentImages)
                .Include(x => x.City)
                .Include(x => x.District)
                .Include(x => x.Favorites)
                .ToList()
                .Where(x => GetDistance(lat, lng, x.Lat, x.Lng) <= 100)
                .Select(c => new AdsListDto
                {
                    adsId = c.Id,
                    cityName = lang == "ar" ? c.City.NameAr : c.City.NameEn,
                    description = c.Description,
                    date = HelperDate.FormatDate(c.CreationDate, lang),
                    districtName = lang == "ar" ? c.District.NameAr : c.District.NameEn,
                    image = c.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).FirstOrDefault() : null,
                    images = c.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).ToList() : null,
                    isFavorite = c.Favorites.Where(d => d.UserId == _currentUserService.UserId && d.AdsId == c.Id).Any(),
                    isSpecial = c.IsSpecial,
                    lat = c.Lat,
                    lng = c.Lng,
                    location = c.Location,
                    title = c.Title,
                    price = c.Price,
                    providerName = c.User.user_Name,
                }).OrderByDescending(a => a.adsId).Skip((PageNumber - 1) * maxRows).Take(maxRows).ToList();


            return new BaseResponseDto<List<AdsListDto>>
            {
                Data = ads,
                IsSuccess = true,
                PaginatioData = new PaginationDto
                {
                    TotalItems = _context.AdvertsmentDetails.Where(d => d.UserId != _currentUserService.UserId && d.IsActive).ToList().Where(d => GetDistance(lat, lng, d.Lat, d.Lng) <= 100).Count()
                }
            };


            //var ads = _context.AdvertsmentDetails.Where(d => d.UserId != _currentUserService.UserId && d.IsActive).Select(c => new AdsListDto
            //{
            //    adsId = c.Id,
            //    cityName = lang == "ar" ? c.City.NameAr : c.City.NameEn,
            //    description = c.Description,
            //    date = HelperDate.FormatDate(c.CreationDate, lang),
            //    districtName = lang == "ar" ? c.District.NameAr : c.District.NameEn,
            //    image = c.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).FirstOrDefault() : null,
            //    images = c.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).ToList() : null,
            //    isFavorite = c.Favorites.Where(d => d.UserId == _currentUserService.UserId && d.AdsId == c.Id).Any(),
            //    isSpecial = c.IsSpecial,
            //    lat = c.Lat,
            //    lng = c.Lng,
            //    location = c.Location,
            //    title = c.Title,
            //    price = c.Price,
            //    providerName = c.User.user_Name,

            //}).OrderByDescending(a => a.adsId).ToList();

            //return new BaseResponseDto<List<AdsListDto>>
            //{
            //    Data = ads,
            //    IsSuccess = true,
            //};

        }

        public static double GetDistance(string FromLat, string FromLng, string ToLat, string ToLng) //VM : ViewModel
        {
            //GeoCoordinate using The haversine formula

            // check impty of LatVM , LngVM , Lat and lng
            if (FromLat == "" || FromLng == "" || ToLat == "" || ToLng == "")
                return 0;

            CultureInfo culture = new CultureInfo("en-US"); // I'm assuming English here.


            if (CheckDouble(FromLat) && CheckDouble(FromLng) && CheckDouble(ToLat) && CheckDouble(ToLng))
            {
                // check rang of LatVM , LngVM , Lat and lng
                if (double.Parse(FromLat, culture) > 90 || double.Parse(FromLat, culture) < -90)
                    return 0;
                if (double.Parse(ToLat, culture) > 90 || double.Parse(ToLat, culture) < -90)
                    return 0;
                if (double.Parse(FromLng, culture) > 180 || double.Parse(FromLng, culture) < -180)
                    return 0;
                if (double.Parse(ToLng, culture) > 180 || double.Parse(ToLng, culture) < -180)
                    return 0;


                // calculate distance
                GeoCoordinate userLocation = new GeoCoordinate(double.Parse(FromLat, culture), double.Parse(FromLng, culture)); // userLocation
                GeoCoordinate targetLocation = new GeoCoordinate(double.Parse(ToLat, culture), double.Parse(ToLng, culture));//ResturantLocation
                var distanceBetweenUserAndResturant = (userLocation.GetDistanceTo(targetLocation)) / 1000; //Distance in KM

                return distanceBetweenUserAndResturant; //Adding Distance between User and all Branches in a List    
            }
            else
                return 0;


        }

        public static bool CheckDouble(string text)
        {

            //double n;
            // bool isNumeric = int.TryParse("123", out n);

            //bool isDouble = double.TryParse(text, out _);

            CultureInfo culture = new CultureInfo("en-US"); // I'm assuming English here.
            //double number = Double.Parse("202.667,40", culture);


            bool isDouble = Double.TryParse(text, NumberStyles.Any, culture, out _);

            return isDouble;
        }
        public async Task<BaseResponseDto<List<AdsListDto>>> ListOfAdsByCategoryAndSubCategoryAndTitle(int CategoryId, int SubCategoryId, string Title, int PageSize = 10, int PageNumber = 1)
        {
            var lang = _langService.Lang;

            int maxRows = PageSize;

            var ads = _context.AdvertsmentDetails.Where(d => d.IsActive && d.UserId != _currentUserService.UserId && (d.CategoryId == CategoryId || CategoryId == 0) && (d.SubCategoryId == SubCategoryId || SubCategoryId == 0) && (d.Title.Contains(Title) || Title == null)).Select(c => new AdsListDto
            {
                adsId = c.Id,
                cityName = lang == "ar" ? c.City.NameAr : c.City.NameEn,
                date = HelperDate.FormatDate(c.CreationDate, lang),
                districtName = lang == "ar" ? c.District.NameAr : c.District.NameEn,
                description = c.Description,
                image = c.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).FirstOrDefault() : null,
                images = c.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).ToList() : null,
                isFavorite = c.Favorites.Where(d => d.UserId == _currentUserService.UserId && d.AdsId == c.Id).Any(),
                isSpecial = c.IsSpecial,
                lat = c.Lat,
                lng = c.Lng,
                location = c.Location,
                title = c.Title,
                price = c.Price,
                providerName = c.User.user_Name,
            }).OrderByDescending(a => a.adsId).Skip((PageNumber - 1) * maxRows).Take(maxRows).ToList();


            return new BaseResponseDto<List<AdsListDto>>
            {
                Data = ads,
                IsSuccess = true,
                PaginatioData = new PaginationDto
                {
                    TotalItems = _context.AdvertsmentDetails.Where(d => d.IsActive && d.UserId != _currentUserService.UserId && (d.CategoryId == CategoryId || CategoryId == 0) && (d.SubCategoryId == SubCategoryId || SubCategoryId == 0) && (d.Title.Contains(Title) || Title == null)).Count()
                }
            };
        }

        public async Task<BaseResponseDto<List<SlidersListDto>>> Sliders()
        {
            var sliders = _context.Sliders.Where(d => d.IsActive).Select(c => new SlidersListDto
            {
                id = c.Id,
                image = DashBordUrl.baseUrlHost + c.Image,
                link = c.Link
            }).OrderByDescending(o => o.id).ToList();

            return new BaseResponseDto<List<SlidersListDto>>
            {
                Data = sliders,
                IsSuccess = true,
            };
        }

        public async Task<BaseResponseDto<string>> SubscribeInPackageByWallet(int adsId, int packageId)
        {
            var lang = _langService.Lang;

            var isSubscribeInThisAds = _context.UserPackages.Where(d => d.AdsId == adsId && d.UserId == _currentUserService.UserId && d.PackageId == packageId).Any();

            if (isSubscribeInThisAds)
            {
                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "لقد قمت بالفعل بالاشتراك في هذه الباقة على هذا الإعلان" : "You have already subscribed to this package on this ad."
                };
            }

            var ads = _context.AdvertsmentDetails.Where(d => d.Id == adsId).FirstOrDefault();
            var package = _context.Packages.Where(d => d.Id == packageId).FirstOrDefault();
            var user = _context.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();

            if (user != null && ads != null && package != null)
            {
                if (user.Wallet >= package.Price)
                {
                    user.Wallet -= package.Price;


                    AsasyUserPackage userPackage = new AsasyUserPackage
                    {
                        AdsId = ads.Id,
                        IsPayed = true,
                        TypePay = (int)TypePay.wallet,
                        StatusReview = (int)StatusReviewForSpecialProduct.Waiting,
                        PackageDescriptionAr = package.DescriptionAr,
                        PackageDescriptionEn = package.DescriptionEn,
                        PackageNameAr = package.NameAr,
                        PackageNameEn = package.NameEn,
                        SubscriptionDate = HelperDate.GetCurrentDate(),
                        EndDate = DateTime.Now.AddDays(package.CountDayes),
                        UserId = _currentUserService.UserId,
                        Price = package.Price,
                        CountDayes = package.CountDayes,
                        PackageId = packageId
                    };

                    _context.UserPackages.Add(userPackage);

                    //ads.IsSpecial = true;

                    _context.SaveChanges();

                    return new BaseResponseDto<string>
                    {
                        IsSuccess = true,
                        Message = lang == "ar" ? "تم ارسال طلب تمييز الاعلان بنجاح" : ""
                    };
                }
                else
                {
                    return new BaseResponseDto<string>
                    {
                        Message = lang == "ar" ? "عفوا الرصيد غير كافي لتمييز الاعلان" : "Sorry, there is not enough balance to feature the ad."
                    };
                }
            }
            else
            {
                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
                };
            }
        }

        public async Task<BaseResponseDto<string>> AddAdsToFavorite(int adId)
        {
            var lang = _langService.Lang;
            try
            {
                Favorites favorite = new Favorites
                {
                    AdsId = adId,
                    UserId = _currentUserService.UserId
                };

                _context.Favorites.Add(favorite);
                _context.SaveChanges();

                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                    Message = lang == "ar" ? "تم اضافه الاعلان للمفضله بنجاح" : "The ad has been added to favourites successfully."
                };
            }
            catch (Exception ex)
            {

                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
                };
            }
            throw new NotImplementedException();
        }

        public async Task<BaseResponseDto<string>> RemoveAdFromFavorite(int adId)
        {
            var lang = _langService.Lang;

            var ad = _context.Favorites.Where(d => d.AdsId == adId && d.UserId == _currentUserService.UserId).FirstOrDefault();
            if (ad != null)
            {
                _context.Favorites.Remove(ad);
                _context.SaveChanges();
                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                    Message = lang == "ar" ? "تم حذف الاعلان من المفضله بنجاح" : "The ad has been removed from favourites successfully."
                };

            }
            return new BaseResponseDto<string>
            {
                Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
            };
        }

        public async Task<BaseResponseDto<List<FavoriteListDto>>> FavoriteList()
        {

            var lang = _langService.Lang;
            try
            {


                var ads = _context.Favorites.Where(d => d.UserId == _currentUserService.UserId).Include(a => a.Ads).Select(c => new FavoriteListDto
                {
                    favoriteId = c.Id,
                    adsId = c.AdsId,
                    cityName = lang == "ar" ? c.Ads.City.NameAr : c.Ads.City.NameEn,
                    date = HelperDate.FormatDate(c.Ads.CreationDate, lang),
                    districtName = lang == "ar" ? c.Ads.District.NameAr : c.Ads.District.NameEn,
                    image = c.Ads.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.Ads.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).FirstOrDefault() : null,
                    isFavorite = true,
                    isSpecial = c.Ads.IsSpecial,
                    lat = c.Ads.Lat,
                    lng = c.Ads.Lng,
                    location = c.Ads.Location,
                    title = c.Ads.Title,
                    price = c.Ads.Price,
                    providerName = c.User.user_Name,
                }).OrderByDescending(a => a.favoriteId).ToList();


                return new BaseResponseDto<List<FavoriteListDto>>
                {
                    Data = ads,
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<BaseResponseDto<string>> ReportAds(ReportAdsDto model)
        {
            var lang = _langService.Lang;
            try
            {
                Report report = new Report
                {
                    AdsId = model.adsId,
                    UserId = _currentUserService.UserId,
                    Type = (int)ReportType.Ads,
                    Comment = model.comment
                };


                _context.Reports.Add(report);


                var ads = _context.AdvertsmentDetails.Where(d => d.Id == model.adsId).FirstOrDefault();
                var user = _context.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();

                NotifyUser notify = new NotifyUser
                {
                    UserId = ads.UserId,
                    AdsId = ads.Id,
                    Date = HelperDate.GetCurrentDate(),
                    Show = false,
                    TextAr = " قام العميل " + user.user_Name + " بالابلاغ علي اعلانك " + ads.Title,
                    TextEn = " Client " + user.user_Name + " report your ad " + ads.Title,
                    Type = (int)NotifyTypes.ReportAds
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
                data.Add("info", ads.Id.ToString());
                data.Add("type", notify.Type.ToString());

                var provider = _context.Users.Where(d => d.Id == ads.UserId).FirstOrDefault();

                NotificationHelper
                    .SendPushNewNotificationAsync(parentDeviceIds, data, HelperMsg.creatMessage(provider.Lang, notify.TextAr, notify.TextEn));




                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                    Message = lang == "ar" ? "تم ارسال الابلاغ بنجاح" : "The report has been sent successfully."
                };
            }
            catch (Exception ex)
            {

                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
                };
            }
        }

        public async Task<BaseResponseDto<string>> ReportProvider(ReportProviderDto model)
        {

            var lang = _langService.Lang;
            try
            {
                Report report = new Report
                {
                    ProviderId = model.providerId,
                    UserId = _currentUserService.UserId,
                    Type = (int)ReportType.Provider,
                    Comment = model.comment
                };

                _context.Reports.Add(report);
                var provider = _context.Users.Where(d => d.Id == model.providerId).FirstOrDefault();

                var user = _context.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();

                NotifyUser notify = new NotifyUser
                {
                    UserId = model.providerId,
                    AdsId = 0,
                    Date = HelperDate.GetCurrentDate(),
                    Show = false,
                    TextAr = " قام العميل " + user.user_Name + " بالابلاغ علي ملفك الشخصي ",
                    TextEn = " Client " + user.user_Name + " report your profile ",
                    Type = (int)NotifyTypes.ReportProvider
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
                data.Add("info", model.providerId);
                data.Add("type", notify.Type.ToString());


                NotificationHelper
                    .SendPushNewNotificationAsync(parentDeviceIds, data, HelperMsg.creatMessage(provider.Lang, notify.TextAr, notify.TextEn));


                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                    Message = lang == "ar" ? "تم ارسال الابلاغ بنجاح" : "The report has been sent successfully."
                };
            }
            catch (Exception ex)
            {

                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
                };
            }
        }

        public async Task<BaseResponseDto<string>> FollowAd(int adId)
        {
            var lang = _langService.Lang;

            try
            {
                Follow newFollow = new Follow
                {
                    AdsId = adId,
                    CreationDate = HelperDate.GetCurrentDate(),
                    UserId = _currentUserService.UserId,
                    Type = (int)FollowType.Ads
                };

                _context.Follows.Add(newFollow);
                var ads = _context.AdvertsmentDetails.Where(d => d.Id == adId).FirstOrDefault();

                var user = _context.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();


                NotifyUser notify = new NotifyUser
                {
                    UserId = ads.UserId,
                    AdsId = adId,
                    Date = HelperDate.GetCurrentDate(),
                    Show = false,
                    TextAr = " قام العميل " + user.user_Name + " بمتابعه اعلانك " + ads.Title,
                    TextEn = " Client " + user.user_Name + " follow your ad " + ads.Title,
                    Type = (int)NotifyTypes.FollowAd
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
                data.Add("info", ads.Id.ToString());
                data.Add("type", notify.Type.ToString());

                var provider = _context.Users.Where(d => d.Id == ads.UserId).FirstOrDefault();

                NotificationHelper
                    .SendPushNewNotificationAsync(parentDeviceIds, data, HelperMsg.creatMessage(provider.Lang, notify.TextAr, notify.TextEn));



                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                    Message = lang == "ar" ? "تم متابعه الاعلان بنجاح" : "The ad has been successfully followed."
                };

            }
            catch (Exception ex)
            {

                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
                };
            }
        }

        public async Task<BaseResponseDto<string>> FollowProvider(string providerId)
        {
            var lang = _langService.Lang;

            try
            {
                Follow newFollow = new Follow
                {
                    ProviderId = providerId,
                    CreationDate = HelperDate.GetCurrentDate(),
                    UserId = _currentUserService.UserId,
                    Type = (int)FollowType.Provider
                };

                _context.Follows.Add(newFollow);
                var user = _context.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();
                var provider = _context.Users.Where(d => d.Id == providerId).FirstOrDefault();


                NotifyUser notify = new NotifyUser
                {
                    UserId = providerId,
                    AdsId = 0,
                    Date = HelperDate.GetCurrentDate(),
                    Show = false,
                    TextAr = " قام العميل " + user.user_Name + " بمتابعه ملفك الشخصي ",
                    TextEn = " Client " + user.user_Name + " follow your profile ",
                    Type = (int)NotifyTypes.FollowProvider
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
                data.Add("info", providerId);
                data.Add("type", notify.Type.ToString());


                NotificationHelper
                    .SendPushNewNotificationAsync(parentDeviceIds, data, HelperMsg.creatMessage(provider.Lang, notify.TextAr, notify.TextEn));


                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                    Message = lang == "ar" ? "تم متابعه المعلن بنجاح" : "The provider has been successfully followed."
                };

            }
            catch (Exception ex)
            {

                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
                };
            }
        }

        public async Task<BaseResponseDto<string>> UnFollowAd(int adId)
        {
            var lang = _langService.Lang;

            try
            {

                var ad = _context.Follows.Where(d => d.AdsId == adId && d.UserId == _currentUserService.UserId && d.Type == (int)FollowType.Ads).FirstOrDefault();
                if (ad != null)
                {
                    _context.Follows.Remove(ad);
                    _context.SaveChanges();


                    return new BaseResponseDto<string>
                    {
                        IsSuccess = true,
                        Message = lang == "ar" ? "تم الغاء المتابعه بنجاح" : "The follow-up was successfully cancelled."
                    };
                }
                else
                {
                    return new BaseResponseDto<string>
                    {
                        Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
                    };
                }


            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<BaseResponseDto<string>> UnFollowProvider(string providerId)
        {
            var lang = _langService.Lang;

            var ad = _context.Follows.Where(d => d.ProviderId == providerId && d.UserId == _currentUserService.UserId && d.Type == (int)FollowType.Provider).FirstOrDefault();
            if (ad != null)
            {
                _context.Follows.Remove(ad);
                _context.SaveChanges();


                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                    Message = lang == "ar" ? "تم الغاء المتابعه بنجاح" : "The follow-up was successfully cancelled."
                };
            }
            else
            {
                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
                };
            }
        }

        public async Task<BaseResponseDto<List<FollowsAdsListDto>>> FollowsAds()
        {
            var lang = _langService.Lang;


            var followsList = _context.Follows.Where(d => d.Type == (int)FollowType.Ads && d.UserId == _currentUserService.UserId && !d.Ads.IsDelete).Include(a => a.Ads).Select(c => new FollowsAdsListDto
            {
                followId = c.Id,
                adsId = c.AdsId,
                cityName = lang == "ar" ? c.Ads.City.NameAr : c.Ads.City.NameEn,
                date = HelperDate.FormatDate(c.CreationDate, lang),
                districtName = lang == "ar" ? c.Ads.District.NameAr : c.Ads.District.NameEn,
                image = c.Ads.AdvertsmentImages.Where(n => n.AdsId == c.AdsId).Select(d => DashBordUrl.baseUrlHost + d.Image).FirstOrDefault(),
                isFavorite = c.Ads.Favorites.Where(d => d.AdsId == c.AdsId && d.UserId == _currentUserService.UserId).Any(),
                isSpecial = c.Ads.IsSpecial,
                lat = c.Ads.Lat,
                lng = c.Ads.Lng,
                location = c.Ads.Location,
                price = c.Ads.Price,
                providerName = c.Ads.User.user_Name,
                title = c.Ads.Title
            }).OrderByDescending(f => f.followId).ToList();

            return new BaseResponseDto<List<FollowsAdsListDto>>
            {
                IsSuccess = true,
                Data = followsList
            };

        }

        public async Task<BaseResponseDto<List<FollowsProviderListDto>>> FollowsProvider()
        {
            var lang = _langService.Lang;

            var followList = _context.Follows.Where(d => d.UserId == _currentUserService.UserId && d.Type == (int)FollowType.Provider).Select(c => new FollowsProviderListDto
            {
                followId = c.Id,
                providerImage = DashBordUrl.baseUrlHost + c.Provider.ImgProfile,
                providerName = c.Provider.user_Name,
                providerId = c.ProviderId,
                joiningDate = HelperDate.FormatDate(c.Provider.PublishDate, lang),
                isOnline = c.Provider.IsOnline,
                sumRate = c.Provider.RatesProviders.Where(d => d.ProviderId == c.ProviderId).Select(r => r.Rate).Sum(),
                countRate = c.Provider.RatesProviders.Where(d => d.ProviderId == c.ProviderId).Select(r => r.Rate).Count(),
                lastAppearance = lang == "ar" ? " متاح " + HelperDate.FormatDate(c.Provider.LastAppearance, lang) : " Available " + HelperDate.FormatDate(c.Provider.LastAppearance, lang),
            }).OrderByDescending(f => f.followId).ToList();

            return new BaseResponseDto<List<FollowsProviderListDto>>
            {
                Data = followList,
                IsSuccess = true
            };

        }

        public async Task<BaseResponseDto<List<AdsCommentDto>>> CommentsAds(int adsId, int PageSize = 10, int PageNumber = 1)
        {
            var lang = _langService.Lang;
            int maxRows = PageSize;

            var comments = _context.CommentAds.Where(d => d.AdsId == adsId).Select(n => new AdsCommentDto
            {
                commentId = n.Id,
                comment = n.Comment,
                userName = n.User.user_Name,
                userImage = DashBordUrl.baseUrlHost + n.User.ImgProfile,
                date = HelperDate.FormatDate(n.CreationDate, lang),
                replaies = n.ReplaiesComments.Select(a => new ReplaiesCommentDto
                {
                    commentId = a.Id,
                    comment = a.Replay,
                    userName = a.User.user_Name,
                    date = HelperDate.FormatDate(a.CreationDate, lang),
                    userImage = DashBordUrl.baseUrlHost + a.User.ImgProfile,
                }).OrderByDescending(o => o.commentId).ToList()
            }).OrderByDescending(o => o.commentId).Skip((PageNumber - 1) * maxRows).Take(maxRows).ToList();


            return new BaseResponseDto<List<AdsCommentDto>>
            {
                Data = comments,
                IsSuccess = true
            };
        }

        public async Task<BaseResponseDto<ProviderInfoDto>> ProviderInfo(string providerId, int PageSize = 10, int PageNumber = 1)
        {
            var lang = _langService.Lang;

            int maxRows = PageSize;

            var provider = _context.Users.Where(d => d.Id == providerId).Select(c => new ProviderInfoDto
            {
                providerId = c.Id,
                providerName = c.user_Name,
                imageProfile = DashBordUrl.baseUrlHost + c.ImgProfile,
                isFollow = c.FollowProviders.Where(d => d.UserId == _currentUserService.UserId && d.ProviderId == providerId && d.Type == (int)FollowType.Provider).Any(),
                joiningDate = HelperDate.FormatDate(c.PublishDate, lang),
                comments = c.RatesProviders.Where(d => d.ProviderId == providerId).Select(n => new CommentsListDto
                {
                    commentId = n.Id,
                    comment = n.Comment,
                    rate = n.Rate,
                    userName = n.User.user_Name,
                    userImage = DashBordUrl.baseUrlHost + n.User.ImgProfile
                }).OrderByDescending(o => o.commentId).Skip((PageNumber - 1) * maxRows).Take(maxRows).ToList(),
                sumRate = c.RatesProviders.Where(d => d.ProviderId == providerId).Select(r => r.Rate).Sum(),
                countRate = c.RatesProviders.Where(d => d.ProviderId == providerId).Select(r => r.Rate).Count(),
                //lastAppearance = lang == "ar" ? " متاح " + HelperDate.FormatDate(c.LastAppearance, lang) : " Available " + HelperDate.FormatDate(c.LastAppearance, lang),
                lastAppearance = HelperDate.FormatDate(c.LastAppearance, lang),
                isOnline = c.IsOnline,
                providerAds = c.Ads.Where(d => d.UserId == providerId && d.IsActive).Select(n => new AdsListDto
                {
                    adsId = n.Id,
                    cityName = lang == "ar" ? n.City.NameAr : n.City.NameEn,
                    date = HelperDate.FormatDate(n.CreationDate, lang),
                    districtName = lang == "ar" ? n.District.NameAr : n.District.NameEn,
                    description = n.Description,
                    image = n.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? n.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).FirstOrDefault() : null,
                    images = n.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? n.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).ToList() : null,
                    isFavorite = c.Favorites.Where(d => d.UserId == _currentUserService.UserId && d.AdsId == n.Id).Any(),
                    isSpecial = n.IsSpecial,
                    lat = c.Lat,
                    lng = c.Lng,
                    location = c.Location,
                    title = n.Title,
                    price = n.Price,
                    providerName = n.User.user_Name,

                }).OrderByDescending(o => o.adsId).ToList(),
                chatId = _context.Chats.Where(d => (d.UserId == _currentUserService.UserId && d.ProviderId == providerId) || (d.UserId == providerId && d.ProviderId == _currentUserService.UserId)).Select(n => n.Id).FirstOrDefault()
            }).FirstOrDefault();


            return new BaseResponseDto<ProviderInfoDto>
            {
                IsSuccess = true,
                Data = provider,
                PaginatioData = new PaginationDto
                {
                    TotalItems = _context.UserRates.Where(d => d.ProviderId == providerId).Count()
                }
            };


        }

        public async Task<BaseResponseDto<List<CommentsListDto>>> CommentsProvider(string providerId)
        {
            var comments = _context.UserRates.Where(d => d.ProviderId == providerId).Select(n => new CommentsListDto
            {
                commentId = n.Id,
                comment = n.Comment,
                rate = n.Rate,
                userName = n.User.user_Name,
                userImage = DashBordUrl.baseUrlHost + n.User.ImgProfile

            }).OrderByDescending(o => o.commentId).ToList();

            return new BaseResponseDto<List<CommentsListDto>>
            {
                Data = comments,
                IsSuccess = true
            };

        }

        public async Task<BaseResponseDto<string>> AddNewCommentForAd(int adsId, string comment)
        {
            var lang = _langService.Lang;

            try
            {
                CommentAds commentAds = new CommentAds
                {
                    AdsId = adsId,
                    Comment = comment,
                    CreationDate = HelperDate.GetCurrentDate(),
                    UserId = _currentUserService.UserId
                };

                _context.CommentAds.Add(commentAds);
                _context.SaveChanges();


                var followsThisAd = _context.Follows.Where(d => d.Type == (int)FollowType.Ads && d.AdsId == adsId && d.UserId != _currentUserService.UserId).ToList();
                var ad = _context.AdvertsmentDetails.Where(d => d.Id == adsId).FirstOrDefault();

                foreach (var item in followsThisAd)
                {
                    NotifyUser notify = new NotifyUser
                    {
                        UserId = item.UserId,
                        AdsId = item.AdsId,
                        Date = HelperDate.GetCurrentDate(),
                        TextAr = " تم اضافه تعليق جديد علي الاعلان " + ad.Title,
                        TextEn = "A new comment has been added to the ad " + ad.Title,
                        Show = false
                    };

                    _context.NotifyUsers.Add(notify);


                    var parentDeviceIds = await _context.DeviceIds.Where(x => x.UserId == notify.UserId).Select(x => new DeviceIdModel
                    {
                        DeviceId = x.DeviceId_,
                        DeviceType = x.DeviceType,
                        FkUser = x.ProjectName,
                        ProjectName = x.ProjectName
                    }).ToListAsync();

                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data.Add("info", notify.AdsId.ToString());
                    data.Add("type", notify.Type.ToString());

                    var user = _context.Users.Where(d => d.Id == notify.UserId).FirstOrDefault();
                    if (user != null)
                    {
                        NotificationHelper
                            .SendPushNewNotificationAsync(parentDeviceIds, data, HelperMsg.creatMessage(user.Lang, notify.TextAr, notify.TextEn));
                    }

                }
                _context.SaveChanges();

                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                    Message = lang == "ar" ? "تم اضافه التعليق بنجاح" : "The comment has been added successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
                };
            }
        }

        public async Task<BaseResponseDto<string>> AddNewRateToProvider(NewRateToProviderDto model)
        {
            var lang = _langService.Lang;
            try
            {
                UserRates userRate = new UserRates
                {
                    UserId = _currentUserService.UserId,
                    Comment = model.comment,
                    CreationDate = HelperDate.GetCurrentDate(),
                    ProviderId = model.providerId,
                    Rate = model.rate
                };

                _context.UserRates.Add(userRate);

                NotifyUser notify = new NotifyUser
                {
                    UserId = model.providerId,
                    AdsId = 0,
                    Date = HelperDate.GetCurrentDate(),
                    TextAr = "  تم اضافه تقييم جديد لك علي ملفك الشخصي ",
                    TextEn = "A new rating has been added to your profile.",
                    Show = false,
                    Type = (int)NotifyTypes.AddRateToUser
                };

                _context.NotifyUsers.Add(notify);
                _context.SaveChanges();


                var parentDeviceIds = await _context.DeviceIds.Where(x => x.UserId == notify.UserId).Select(x => new DeviceIdModel
                {
                    DeviceId = x.DeviceId_,
                    DeviceType = x.DeviceType,
                    FkUser = x.ProjectName,
                    ProjectName = x.ProjectName
                }).ToListAsync();

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("info", notify.UserId);
                data.Add("type", notify.Type.ToString());

                var user = _context.Users.Where(d => d.Id == notify.UserId).FirstOrDefault();
                if (user != null)
                {
                    NotificationHelper
                        .SendPushNewNotificationAsync(parentDeviceIds, data, HelperMsg.creatMessage(user.Lang, notify.TextAr, notify.TextEn));
                }



                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                    Message = lang == "ar" ? "تم اضافه التقييم بنجاح" : "The rating has been added successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
                };


            }
        }

        public async Task<BaseResponseDto<List<AdsListDto>>> FiltrationAds(FiltrationAdsDto model)
        {
            var lang = _langService.Lang;

            //var ads = _context.AdvertsmentDetails.Where(d => d.UserId != _currentUserService.UserId && d.IsActive && !d.User.IsDeleted);
            var ads = _context.AdvertsmentDetails.Where(d => d.IsActive && !d.User.IsDeleted);
            PaginationDto pagination = new PaginationDto();
            if (model.regionId != 0)
            {
                ads = ads.Where(d => d.RegionId == model.regionId);
                //pagination.TotalItems = _context.AdvertsmentDetails.Where(d => d.UserId != _currentUserService.UserId && d.IsActive && !d.User.IsDeleted && d.RegionId == model.regionId).Count();
            }

            if (model.from != default && model.to != default)
            {
                ads = ads.Where(d => d.CreationDate.Date >= model.from.Date && d.CreationDate.Date <= model.to.Date);
                //pagination.TotalItems = _context.AdvertsmentDetails.Where(d => d.UserId != _currentUserService.UserId && d.IsActive && !d.User.IsDeleted && d.CreationDate.Date >= model.from.Date && d.CreationDate.Date <= model.to.Date).Count();

            }

            if (model.isContainImages)
            {
                ads = ads.Where(d => d.AdvertsmentImages.Any());
                //pagination.TotalItems = _context.AdvertsmentDetails.Where(d => d.UserId != _currentUserService.UserId && d.IsActive && !d.User.IsDeleted && d.AdvertsmentImages.Any()).Count();

            }

            if (model.categoryId != 0)
            {
                ads = ads.Where(d => d.CategoryId == model.categoryId);
                //pagination.TotalItems = _context.AdvertsmentDetails.Where(d => d.UserId != _currentUserService.UserId && d.IsActive && !d.User.IsDeleted && d.CategoryId == model.categoryId).Count();

            }
            if (model.subCategories != null)
            {
                ads = ads.Where(d => model.subCategories.Contains(d.SubCategoryId));
                //pagination.TotalItems = _context.AdvertsmentDetails.Where(d => d.UserId != _currentUserService.UserId && d.IsActive && !d.User.IsDeleted && d.SubCategoryId == model.subCategoryId).Count();

            }

            pagination.TotalItems = ads.Count();

            int maxRows = model.PageSize;
            var allAds = ads.Select(c => new AdsListDto
            {
                adsId = c.Id,
                cityName = lang == "ar" ? c.City.NameAr : c.City.NameEn,
                date = HelperDate.FormatDate(c.CreationDate, lang),
                districtName = lang == "ar" ? c.District.NameAr : c.District.NameEn,
                description = c.Description,
                image = c.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).FirstOrDefault() : null,
                images = c.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).ToList() : null,
                isFavorite = c.Favorites.Where(d => d.UserId == _currentUserService.UserId && d.AdsId == c.Id).Any(),
                isSpecial = c.IsSpecial,
                lat = c.Lat,
                lng = c.Lng,
                location = c.Location,
                title = c.Title,
                price = c.Price,
                providerName = c.User.user_Name,

            }).Skip((model.PageNumber - 1) * maxRows).Take(maxRows).ToList();


            if (model.isLatest)
            {
                allAds = allAds.OrderByDescending(o => o.adsId).Skip((model.PageNumber - 1) * maxRows).Take(maxRows).ToList();

            }

            return new BaseResponseDto<List<AdsListDto>>
            {
                Data = allAds,
                IsSuccess = true,
                PaginatioData = pagination
            };

        }

        public async Task<BaseResponseDto<List<AdsListDto>>> MyAds(int PageSize = 10, int PageNumber = 1)
        {
            var lang = _langService.Lang;
            int maxRows = PageSize;
            var ads = _context.AdvertsmentDetails.Where(d => d.UserId == _currentUserService.UserId && d.IsActive).Select(c => new AdsListDto
            {
                adsId = c.Id,
                cityName = lang == "ar" ? c.City.NameAr : c.City.NameEn,
                date = HelperDate.FormatDate(c.CreationDate, lang),
                districtName = lang == "ar" ? c.District.NameAr : c.District.NameEn,
                description = c.Description,
                image = c.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).FirstOrDefault() : null,
                images = c.AdvertsmentImages.Select(n => n.Image).FirstOrDefault() != null ? c.AdvertsmentImages.Select(n => DashBordUrl.baseUrlHost + n.Image).ToList() : null,
                isFavorite = c.Favorites.Where(d => d.UserId == _currentUserService.UserId && d.AdsId == c.Id).Any(),
                isSpecial = c.IsSpecial,
                lat = c.Lat,
                lng = c.Lng,
                location = c.Location,
                title = c.Title,
                price = c.Price,
                providerName = c.User.user_Name,

            }).OrderByDescending(a => a.adsId).Skip((PageNumber - 1) * maxRows).Take(maxRows).ToList();

            return new BaseResponseDto<List<AdsListDto>>
            {
                Data = ads,
                IsSuccess = true,
                PaginatioData = new PaginationDto
                {
                    TotalItems = _context.AdvertsmentDetails.Where(d => d.UserId == _currentUserService.UserId && d.IsActive).Count()
                }
            };

        }

        public async Task<BaseResponseDto<string>> RemoveAds(int adsId)
        {
            var lang = _langService.Lang;

            var ad = _context.AdvertsmentDetails.Where(d => d.Id == adsId).FirstOrDefault();
            if (ad != null)
            {
                ad.IsDelete = true;
                _context.SaveChanges();

                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                    Message = lang == "ar" ? "تم الحذف بنجاح" : "Deleted successfully"
                };
            }
            else
            {
                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
                };
            }
        }

        public async Task<BaseResponseDto<AdsDetailsToUpdateDto>> AdsDetailsToUpdate(int adsId)
        {
            var ads = _context.AdvertsmentDetails.Where(d => d.Id == adsId).Select(c => new AdsDetailsToUpdateDto
            {
                adsId = c.Id,
                categoryId = c.CategoryId,
                cityId = c.CityId,
                description = c.Description,
                title = c.Title,
                districtId = c.DistrictId,
                showPhone = c.ShowPhone,
                lat = c.Lat,
                lng = c.Lng,
                location = c.Location,
                phoneCode = c.PhoneCode,
                phoneNumber = c.Phone,
                price = c.Price,
                regionId = c.RegionId,
                subCategoryId = c.SubCategoryId,
                statusProduct = c.StatusProduct,
                images = c.AdvertsmentImages.Select(n => new AdImagesDto
                {
                    imageId = n.Id,
                    image = DashBordUrl.baseUrlHost + n.Image
                }).ToList()
            }).FirstOrDefault();

            return new BaseResponseDto<AdsDetailsToUpdateDto>
            {
                Data = ads,
                IsSuccess = true
            };
        }






        public async Task<BaseResponseDto<string>> UpdateAds(UpdateAdsDto model)
        {
            var lang = _langService.Lang;
            try
            {
                var oldImagesCount = 0;
                var newImagesCount = 0;

                if (model.oldImages != null)
                    oldImagesCount = model.oldImages.Count();

                if (model.newImages != null)
                    newImagesCount = model.newImages.Count();

                var totalCountImages = oldImagesCount + newImagesCount;

                if (totalCountImages > 6)
                {
                    return new BaseResponseDto<string>
                    {
                        Message = lang == "ar" ? "لا يمكنك التعديل بأكثر من 6 صور للاعلان" : "You cannot edit more than 6 images for the ad."
                    };
                }

                var ad = _context.AdvertsmentDetails.Where(d => d.Id == model.adsId).FirstOrDefault();

                if (ad != null)
                {
                    ad.RegionId = model.regionId;
                    ad.CityId = model.cityId;
                    ad.RegionId = model.regionId;
                    ad.DistrictId = model.districtId;
                    ad.Lat = model.lat;
                    ad.Lng = model.lng;
                    ad.Location = model.location;
                    ad.Price = model.price;
                    ad.Description = model.description;
                    ad.CategoryId = model.categoryId;
                    ad.SubCategoryId = model.subCategoryId;
                    ad.StatusProduct = model.statusProduct;
                    ad.Phone = model.phoneNumber;
                    ad.PhoneCode = model.phoneCode;
                    ad.ShowPhone = model.showPhone;
                    ad.Title = model.title;

                    List<string> images = new List<string>();

                    if (model.oldImages != null)
                    {
                        foreach (var item in model.oldImages)
                        {
                            var oldImg = _context.AdvertsmentImages.Where(d => d.Id == item).FirstOrDefault();
                            images.Add(oldImg.Image);

                        }
                    }

                    if (model.newImages != null)
                    {
                        foreach (var item in model.newImages)
                        {
                            var img = _uploadImage.Upload(item, (int)FileName.Advertisment);
                            images.Add(img);
                        }
                    }
                    //var oldImages = _context.AdvertsmentImages.Where(d => d.AdsId == model.adsId).ToList();

                    //_context.AdvertsmentImages.RemoveRange(oldImages);



                    var oldImages = _context.AdvertsmentImages.Where(d => d.AdsId == model.adsId).ToList();

                    // Remove physical files
                    foreach (var image in oldImages)
                    {
                        if (!string.IsNullOrEmpty(image.Image))
                        {
                            // Get absolute file path
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Image.TrimStart('/'));

                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                    }

                    // Remove from DB
                    _context.AdvertsmentImages.RemoveRange(oldImages);
                    _context.SaveChanges();




                    _context.SaveChanges();

                    if (images != null)
                    {
                        foreach (var item in images)
                        {
                            AdvertsmentImages adImage = new AdvertsmentImages
                            {
                                AdsId = model.adsId,
                                Image = item,
                            };

                            _context.AdvertsmentImages.Add(adImage);
                        }
                    }

                    _context.SaveChanges();

                    return new BaseResponseDto<string>
                    {
                        IsSuccess = true,
                        Message = lang == "ar" ? "تم التعديل بنجاح" : "Modified successfully"
                    };
                }
                else
                {
                    return new BaseResponseDto<string>
                    {
                        Message = lang == "ar" ? "عفوا حدث خطأ ما فضلا تأكد من البيانات" : "Sorry, something went wrong. Please check the data."
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "عفوا حدث خطأ ما فضلا تأكد من البيانات" : "Sorry, something went wrong. Please check the data."
                };
            }
        }

        public async Task<BaseResponseDto<List<MyPackagesDetailsDto>>> MyPackagesList()
        {
            var lang = _langService.Lang;

            var myPackages = _context.UserPackages.Where(d => d.UserId == _currentUserService.UserId).Include(d => d.Ads).Select(c => new MyPackagesDetailsDto
            {
                userPackageId = c.Id,
                adsId = c.AdsId,
                description = lang == "ar" ? c.PackageDescriptionAr : c.PackageDescriptionEn,
                name = lang == "ar" ? c.PackageNameAr : c.PackageNameEn,
                packageId = c.PackageId,
                price = c.Price,
                remainingDays = (c.EndDate - HelperDate.GetCurrentDate(3)).Days > 0 ? (c.EndDate - HelperDate.GetCurrentDate(3)).Days : 0,
                adTitle = c.Ads.Title
            }).OrderByDescending(o => o.userPackageId).ToList();

            return new BaseResponseDto<List<MyPackagesDetailsDto>>
            {
                Data = myPackages,
                IsSuccess = true
            };

        }

        public async Task<BaseResponseDto<string>> ReNewSubscribeInPackageByWallet(int userPackageId)
        {
            var lang = _langService.Lang;

            try
            {


                var userPackage = _context.UserPackages.Where(d => d.Id == userPackageId).FirstOrDefault();

                if (userPackage != null)
                {
                    var package = _context.Packages.Where(d => d.Id == userPackage.PackageId).FirstOrDefault();

                    if (package != null)
                    {
                        if (package.CountDayes != userPackage.CountDayes)
                        {
                            return new BaseResponseDto<string>
                            {
                                Message = lang == "ar" ? "عذرا، لقد تم تغيير مدة الباقة. يرجى الاشتراك في الباقة مرة أخرى إذا كنت ترغب في ذلك." : "Sorry, the package duration has been changed. Please subscribe to the package again if you wish."
                            };
                        }

                        if (package.Price != userPackage.Price)
                        {
                            return new BaseResponseDto<string>
                            {
                                Message = lang == "ar" ? "عذرا، لقد تم تغيير سعر الباقة. يرجى الاشتراك في الباقة مرة أخرى إذا كنت ترغب في ذلك.." : "Sorry, the package price has changed. Please subscribe to the package again if you wish."
                            };
                        }

                        var user = _context.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();

                        if (user.Wallet >= userPackage.Price)
                        {
                            user.Wallet -= userPackage.Price;
                            userPackage.PackageDescriptionAr = package.DescriptionAr;
                            userPackage.PackageDescriptionEn = package.DescriptionEn;
                            userPackage.PackageNameEn = package.NameEn;
                            userPackage.PackageNameAr = package.NameAr;
                            userPackage.SubscriptionDate = HelperDate.GetCurrentDate();
                            userPackage.EndDate = DateTime.Now.AddDays(package.CountDayes);

                            var ad = _context.AdvertsmentDetails.Where(d => d.Id == userPackage.AdsId).FirstOrDefault();

                            ad.IsSpecial = true;

                            _context.SaveChanges();



                            return new BaseResponseDto<string>
                            {
                                IsSuccess = true,
                                Message = lang == "ar" ? "تم تجديد الاشتراك في الباقه بنجاح" : "The package subscription has been renewed successfully."
                            };

                        }
                        else
                        {
                            return new BaseResponseDto<string>
                            {
                                Message = lang == "ar" ? "عفوا الرصيد غير كافي لتمييز الاعلان" : "Sorry, there is not enough balance to feature the ad."
                            };
                        }
                    }
                    else
                    {
                        return new BaseResponseDto<string>
                        {
                            Message = lang == "ar" ? " فضلا تأكد من البيانات" : "Please check the data."
                        };
                    }
                }
                else
                {
                    return new BaseResponseDto<string>
                    {
                        Message = lang == "ar" ? " فضلا تأكد من البيانات" : "Please check the data."
                    };

                }

            }
            catch (Exception)
            {

                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? " فضلا تأكد من البيانات" : "Please check the data."
                };
            }
        }

        public async Task<BaseResponseDto<string>> CheckIsPackageIsEndedOrNot()
        {
            try
            {
                var userPackges = _context.UserPackages.Where(d => d.EndDate.Date <= HelperDate.GetCurrentDate(3).Date && d.Ads.IsSpecial).ToList();
                foreach (var userPackage in userPackges)
                {
                    var ads = _context.AdvertsmentDetails.Where(d => d.Id == userPackage.AdsId).FirstOrDefault();
                    if (ads != null)
                    {
                        ads.IsSpecial = false;



                        NotifyUser notify = new NotifyUser
                        {
                            UserId = ads.UserId,
                            AdsId = ads.Id,
                            Date = HelperDate.GetCurrentDate(),
                            Show = false,
                            TextAr = "تم انهاء باقه تمييز اعلانك " + ads.Title,
                            TextEn = "Your ad promotion package has been terminated " + ads.Title,
                            Type = (int)NotifyTypes.SubscribeInPackageIsEnded
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
                        data.Add("info", ads.Id.ToString());
                        data.Add("type", notify.Type.ToString());

                        var provider = _context.Users.Where(d => d.Id == ads.UserId).FirstOrDefault();

                        if (provider != null)
                        {
                            NotificationHelper
                                .SendPushNewNotificationAsync(parentDeviceIds, data, HelperMsg.creatMessage(provider.Lang, notify.TextAr, notify.TextEn));

                        }
                    }

                }
                _context.SaveChanges();
                return new BaseResponseDto<string>
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {

                return new BaseResponseDto<string>
                {
                    IsSuccess = false
                };
            }
        }

        public async Task<BaseResponseDto<string>> WalletCharging(double balance)
        {
            var lang = _langService.Lang;

            var user = _context.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();

            user.Wallet += balance;
            _context.SaveChanges();

            return new BaseResponseDto<string>
            {
                Message = lang == "ar" ? "تم شحن المحفظه" : "The wallet has been charged.",
                IsSuccess = true
            };
        }

        public async Task<BaseResponseDto<string>> AddNewReplayForComment(int commentId, string comment)
        {
            var lang = _langService.Lang;

            try
            {
                ReplaiesComment newReplay = new ReplaiesComment
                {
                    Replay = comment,
                    CommentId = commentId,
                    CreationDate = HelperDate.GetCurrentDate(),
                    UserId = _currentUserService.UserId
                };
                _context.ReplaiesComments.Add(newReplay);

                var mainComment = _context.CommentAds.Where(d => d.Id == commentId).FirstOrDefault();
                var followsThisAd = _context.Follows.Where(d => d.Type == (int)FollowType.Ads && d.AdsId == mainComment.AdsId && d.UserId != _currentUserService.UserId).ToList();
                var ad = _context.AdvertsmentDetails.Where(d => d.Id == mainComment.AdsId).FirstOrDefault();

                foreach (var item in followsThisAd)
                {
                    NotifyUser notify = new NotifyUser
                    {
                        UserId = item.UserId,
                        AdsId = item.AdsId,
                        Date = HelperDate.GetCurrentDate(),
                        TextAr = " تم اضافه رد جديد علي التعليق " + mainComment.Comment + " في الاعلان " + ad.Title,
                        TextEn = "A new replay has been added to the comment " + mainComment.Comment + " in Ad " + ad.Title,
                        Show = false,
                        Type = (int)NotifyTypes.NewComment
                    };

                    _context.NotifyUsers.Add(notify);


                    var parentDeviceIds = await _context.DeviceIds.Where(x => x.UserId == notify.UserId).Select(x => new DeviceIdModel
                    {
                        DeviceId = x.DeviceId_,
                        DeviceType = x.DeviceType,
                        FkUser = x.ProjectName,
                        ProjectName = x.ProjectName
                    }).ToListAsync();

                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data.Add("info", notify.AdsId.ToString());
                    data.Add("type", notify.Type.ToString());

                    var user = _context.Users.Where(d => d.Id == notify.UserId).FirstOrDefault();
                    if (user != null)
                    {
                        NotificationHelper
                            .SendPushNewNotificationAsync(parentDeviceIds, data, HelperMsg.creatMessage(user.Lang, notify.TextAr, notify.TextEn));
                    }

                }




                _context.SaveChanges();


                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                    Message = lang == "ar" ? "تم اضافه الرد علي التعليق بنجاح" : "The reply to the comment has been added successfully"
                };

            }
            catch (Exception ex)
            {

                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, check data"
                };
            }
        }
    }
}
