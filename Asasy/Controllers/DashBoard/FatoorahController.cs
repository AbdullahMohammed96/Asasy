using AAITPayment.MyFatoorah.Model;
using AAITPayment.MyFatoorah;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Asasy.Persistence;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using AAITHelper.Enums;
using Asasy.Domain.DTO;
using AAITHelper;
using Asasy.Domain.Entities.AsasyPackages;

namespace Asasy.Controllers.DashBoard
{
    public class FatoorahController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IMyFatoorah _myFatoorah;

        public FatoorahController(ApplicationDbContext context, IMyFatoorah myFatoorah)
        {
            _context = context;
            _myFatoorah = myFatoorah;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region MyFatoorahBase
        public async Task<IActionResult> InitiateMyFatoorahPage(string userId, double amount, int? adsId, int? packageId, int? userPackageId, int type)
        {
            MyFatoorahCallBackUrlConfig myFatoorahCallBackUrlConfig = new MyFatoorahCallBackUrlConfig
            {
                SuccessActionName = "MyFatoorahWebSubmit",  // دى الفانكشن اللى بيروح عليها لما عملية الدفع بتم
                ControllerName = "Fatoorah", // اسم ال Controller اللى موجود فيه الفانكشن اللى هيروح عليها
                LiveHostServer = DashBordUrl.baseUrlHost, // دا لازم يكون real domain والا الدفع مش هيشتغل اصلا
                                                          // علشان لما بت Call ExecutePayment => بيروح على ال URL اللى موحود فى CallBackURL
                FailActionName = "Fail", // اسم الاكشن اللى بيروح عليه لو حصل FAIL
            };

            //اى QueryString هتبعته لل InitiateMyFatoorahPage هضيفه هنا 
            RouteValues routeValues = new RouteValues
            {
                //PackageId = packageId,
                UserId = userId,
                Type = type,
                Amount = amount,
                adsId = adsId,
                packageId = packageId,
                userPackageId = userPackageId,
            };

            ViewBag.RouteValues = routeValues; // ببعتهم منفصلين علشان نسهلم ارسالهم من ال view لل ExecutePayment 
            _myFatoorah = new MyFatoorah(myFatoorahCallBackUrlConfig, routeValues);

            return View("Index", _myFatoorah.MyFatoorahCallBackUrlConfig);
        }




        // دا الاكشن اللى بيعمل Call InitiatePayment => الخاصة ببوابة الدفع
        // دى بترجعلك Json فيها كل ال Payments اللى عميل مشترك فيها
        public async Task<IActionResult> InitiatePayment(string amount)
        {
            var paymentMethods = await _myFatoorah.InitiatePayment(amount);
            return Json(paymentMethods);
        }



        //دا الاكشن اللى اللى بيتعمل Call اول ما يتم اختيار وسيلة الدفع => ex. VISA, MasterCard, Mada...etc
        public async Task<ActionResult> ExecutePayment(ExecutePaymentModel executePaymentModel)
        {
            var result = await IsOperationSuccess(executePaymentModel.adsId, executePaymentModel.packageId, executePaymentModel.userPackageId, executePaymentModel.InvoiceValue, executePaymentModel.UserId, executePaymentModel.Type);

            if (!result)
                return Json(new ExecutePaymentResponse { IsSuccess = true, Data = new DataResponse { PaymentURL = "/Fatoorah/Fail", IsDirectPayment = false } });

            var resultResponse = await _myFatoorah.ExecutePayment(executePaymentModel);

            //Check if invoice created successfully --> Hint This endpoint not required
            var getPaymentStatusResponse = await _myFatoorah.GetPaymentStatus(resultResponse.Data.InvoiceId.ToString(), "InvoiceId");

            return Json(resultResponse);
        }



        [HttpGet]
        // This is action which callled from CallBackURL after ExecutePayment
        public async Task<IActionResult> MyFatoorahWebSubmit(string userId, int? adsId, int? packageId, int? userPackageId, int? type, double amount, string? paymentId, string? id)
        {
            using var dbContextTransaction = _context.Database.BeginTransaction();
            try
            {
                //Please note that MyFatoorah will always add the PaymentId to either the CallBackUrl / ErrorUrl
                //This is done to enable you to call the GetPaymentStatus endpoint to update the status of the order on your own website.  
                var getPaymentStatusResponse = await _myFatoorah.GetPaymentStatus(paymentId, "PaymentId");

                var resultResponse = JsonConvert.DeserializeObject<ExecutePaymentResponse>(getPaymentStatusResponse);

                if (resultResponse.Data.InvoiceStatus == "Paid")
                {
                    #region Code
                    var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
                    if (amount != 0 && user != null && type != null)
                    {
                        //Check type pay is Package
                        if (type == PaymentTypeFor.SubsSubscribeInPackage.ToNumber())
                        {
                            if (adsId != null && packageId != null)
                            {
                                var ads = _context.AdvertsmentDetails.Where(d => d.Id == adsId).FirstOrDefault();
                                var package = _context.Packages.Where(d => d.Id == packageId).FirstOrDefault();
                                if (user != null && ads != null && package != null)
                                {
                                    AsasyUserPackage userPackage = new AsasyUserPackage
                                    {
                                        AdsId = ads.Id,
                                        IsPayed = true,
                                        TypePay = (int)TypePay.online,
                                        StatusReview = (int)StatusReviewForSpecialProduct.Waiting,
                                        PackageDescriptionAr = package.DescriptionAr,
                                        PackageDescriptionEn = package.DescriptionEn,
                                        PackageNameAr = package.NameAr,
                                        PackageNameEn = package.NameEn,
                                        SubscriptionDate = HelperDate.GetCurrentDate(),
                                        EndDate = DateTime.Now.AddDays(package.CountDayes),
                                        UserId = userId,
                                        Price = package.Price,
                                        CountDayes = package.CountDayes,
                                        PackageId = packageId.Value
                                    };

                                    _context.UserPackages.Add(userPackage);

                                    //ads.IsSpecial = true;
                                    await _context.SaveChangesAsync();
                                    dbContextTransaction.Commit();

                                    return RedirectToAction(nameof(Success), new { type = type });


                                }

                            }


                            return RedirectToAction(nameof(Fail), new { type = type });
                        }
                        if (type == PaymentTypeFor.ReNewSubscribeInPackage.ToNumber())
                        {
                            var userPackage = _context.UserPackages.Where(d => d.Id == userPackageId).FirstOrDefault();
                            if (userPackage != null)
                            {
                                var package = _context.Packages.Where(d => d.Id == userPackage.PackageId).FirstOrDefault();

                                if (package != null)
                                {
                                    userPackage.PackageDescriptionAr = package.DescriptionAr;
                                    userPackage.PackageDescriptionEn = package.DescriptionEn;
                                    userPackage.PackageNameEn = package.NameEn;
                                    userPackage.PackageNameAr = package.NameAr;
                                    userPackage.SubscriptionDate = HelperDate.GetCurrentDate();
                                    userPackage.EndDate = DateTime.Now.AddDays(package.CountDayes);
                                    userPackage.IsPayed = true;
                                    userPackage.TypePay = (int)TypePay.online;

                                    var ad = _context.AdvertsmentDetails.Where(d => d.Id == userPackage.AdsId).FirstOrDefault();

                                    ad.IsSpecial = true;


                                    await _context.SaveChangesAsync();
                                    dbContextTransaction.Commit();
                                    return RedirectToAction(nameof(Success), new { type = type });

                                }
                            }


                            return RedirectToAction(nameof(Fail), new { type = type });

                        }
                        if (type == PaymentTypeFor.PayAppCommission.ToNumber())
                        {
                            Payments newPayments = new Payments
                            {
                                Paid = amount,
                                TypePay = (int)TypePay.online,
                                UserId = user.Id
                            };
                            _context.PaymentUsers.Add(newPayments);

                            await _context.SaveChangesAsync();
                            dbContextTransaction.Commit();
                            return RedirectToAction(nameof(Success), new { type = type });
                        }

                        if (type == PaymentTypeFor.Wallet.ToNumber())
                        {
                            user.Wallet += amount;
                            await _context.SaveChangesAsync();
                            dbContextTransaction.Commit();
                            return RedirectToAction(nameof(Success), new { type = type });

                        }


                    }

                    #endregion
                }

                dbContextTransaction.Rollback();
                // Log exception if something went wrong
                await _context.LogExption.AddAsync(new LogExption
                {
                    Date = DateTime.Now,
                    UserId = userId,
                    ServiceName = "MyFatoorahWebSubmit",
                    Exption = "Something went wrong"
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Fail));
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();

                await _context.LogExption.AddAsync(new LogExption
                {
                    Date = DateTime.Now,
                    UserId = userId,
                    ServiceName = "MyFatoorahWebSubmit",
                    Exption = ex.Message
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Fail));
            }
        }




        //View of actions payments
        public ActionResult Success(int type)
        {

            ViewBag.Message = GetStatusSuccess(type);
            return View();
        }

        private static string GetStatusSuccess(int type)
        {
            string result = "";

            if (type == (int)PaymentTypeFor.SubsSubscribeInPackage)
            {
                result = "تم الدفع بنجاح و تم ارسال طلب تمييزك للاعلان للاداره";
            }
            else if (type == (int)PaymentTypeFor.ReNewSubscribeInPackage)
            {
                result = "تم تجديد الاشتراك في الباقه بنجاح";
            }
            else if (type == (int)PaymentTypeFor.PayAppCommission)
            {
                result = "تم دفع عموله التطبيق بنجاح شكرا لامانتك";
            }
            else
            {
                result = "تم شحن المحفظه بنجاح";
            }
            return result;
        }


        public ActionResult Fail()
        {
            return View();
        }
        #endregion




        //الفانكشن دى وظيفتها انها بتشيك على الداتا اللى مبعوته Vaild Or not => قبل ما يحوله اصلا على الصفحة اللى هيكتب فيها بيانات الكارد
        private async Task<bool> IsOperationSuccess(int? adsId, int? packageId, int? userPackageId, double amount, string userId, int type)
        {
            try
            {
                // Check if type in range 0-1 to ensure send vaildType
                // 0 => Package
                // 1 => Wallet
                if (type > 4 || type < 0)
                {
                    return false;
                }
                if (type == PaymentTypeFor.Wallet.ToNumber())
                {
                    return true;
                }

                var userInDb = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (userInDb == null)
                    return false;

                if (type == PaymentTypeFor.SubsSubscribeInPackage.ToNumber())
                {
                    if (adsId != null && packageId != null)
                    {
                        var ad = _context.AdvertsmentDetails.Where(d => d.Id == adsId).FirstOrDefault();
                        if (ad == null)
                            return false;

                        var package = _context.Packages.Where(d => d.Id == packageId).FirstOrDefault();
                        if (package == null)
                            return false;

                        if (package.Price != amount)
                            return false;


                        return true;
                    }

                    if (userPackageId != null)
                    {
                        var userPackage = _context.UserPackages.Where(d => d.Id == userPackageId).FirstOrDefault();

                        if (userPackage == null)
                            return false;

                        var package = _context.Packages.Where(d => d.Id == userPackage.PackageId).FirstOrDefault();
                        if (package != null)
                        {
                            if (package.CountDayes != userPackage.CountDayes)
                            {
                                return false;

                            }

                            if (package.Price != userPackage.Price)
                            {
                                return false;

                            }
                        }
                        else
                        {
                            return false;
                        }


                        return true;
                    }


                    //var packgeInDb = await _context.Packages.SingleOrDefaultAsync(x => x.Id == packageId);
                    //if (packgeInDb is not null)
                    //{
                    //    //Check if TotalAmount equal amount which send from url to ensure he send vaild amount
                    //    if (packgeInDb.Price != amount)
                    //        return false;

                    //    return true;
                    //}
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        private static string Base64Decode(string base64EncodedData)
        {
            if (base64EncodedData == null)
            {
                return "";
            }
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
