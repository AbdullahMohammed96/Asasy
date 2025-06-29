using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asasy.Domain.DTO.More;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Enums;
using Asasy.Persistence;
using Asasy.Service.Contract.More;
using Asasy.Domain.DTO;
using NPOI.OpenXmlFormats.Spreadsheet;
using Asasy.Domain.Common.Helpers;
using Asasy.Service.Api.Contract.Auth;
using Asasy.Service.Api.Contract.Lang;
using AAITHelper;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.Entities.UserTables;
using NPOI.SS.Formula.Functions;

namespace Asasy.Service.Implementation.More
{
    public class MoreService : IMoreService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILangService _langService;
        private readonly IHelper _uploadImage;
        private readonly ICurrentUserService _currentUserService;

        public MoreService(ApplicationDbContext db, ILangService langService, IHelper uploadImage, ICurrentUserService currentUserService)
        {
            _db = db;
            _langService = langService;
            _uploadImage = uploadImage;
            _currentUserService = currentUserService;
        }

        public async Task<AboutUsDto> AboutUs(string lang = "ar")
        {
            AboutUsDto aboutUsDto = new AboutUsDto()
            {
                aboutUs = await _db.Settings
                                   .Select(a => lang == "ar" ? a.AboutUsArClient : a.AboutUsEnClient)
                                   .FirstOrDefaultAsync()
            };

            return aboutUsDto;
        }

        public async Task<List<CommonQuestionsDto>> CommonQuestions(string lang = "ar")
        {
            var question = await _db.Questions
                                    .Select(q => new CommonQuestionsDto()
                                    {
                                        Question = lang == "ar" ? q.QuestionAr : q.QuestionEn,
                                        Answer = lang == "ar" ? q.AnswerAr : q.AnswerEn
                                    }).ToListAsync();

            return question;
        }

        public async Task<TermsAndConditionsDto> TermsAndConditions(string lang = "ar")
        {
            var termsAndCondition = await _db.Settings.Select(t => new TermsAndConditionsDto()
            {
                TermAndCondition = lang == "ar" ? t.CondtionsArClient : t.CondtionsEnClient
            }).FirstOrDefaultAsync();

            return termsAndCondition;
        }

        public async Task<bool> ContactWithUs(ContactWithUsDto contactWithUsDto)
        {
            ContactUs contactWithUs = new ContactUs()
            {
                UserName = contactWithUsDto.UserName,
                Msg = contactWithUsDto.Message,
                Email = contactWithUsDto.Email,
                Date = DateTime.Now
            };

            await _db.AddAsync(contactWithUs);
            return await _db.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<BaseResponseDto<string>> GetWallet()
        {
            var lang = _langService.Lang;

            var user = _db.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();

            if (user != null)
            {
                var wallet = user.Wallet;

                return new BaseResponseDto<string>
                {
                    Data = wallet.ToString(),
                    IsSuccess = true
                };

            }
            else
            {
                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, Check Data"
                };
            }
        }

        public async Task<BaseResponseDto<string>> CalculateCommissionApp(double value)
        {
            var setting = _db.Settings.FirstOrDefault();

            var commission = value * setting.CommissionApp;

            return new BaseResponseDto<string>
            {
                Data = commission.ToString(),
                IsSuccess = true
            };
        }

        public async Task<BaseResponseDto<string>> PayAppCommissionByWallet(double value)
        {
            var lang = _langService.Lang;

            if (value <= 0)
            {
                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا ادخل العموله بشكل صحيح" : "Please enter the commission correctly."
                };
            }

            var user = _db.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();
            if (user != null)
            {
                if (user.Wallet >= value)
                {
                    user.Wallet -= value;

                    Payments newPayments = new Payments
                    {
                        Paid = value,
                        TypePay = (int)TypePay.wallet,
                        UserId = user.Id
                    };
                    _db.PaymentUsers.Add(newPayments);

                    _db.SaveChanges();

                    return new BaseResponseDto<string>
                    {
                        IsSuccess = true,
                        Message = lang == "ar" ? "تم دفع عموله التطبيق بنجاح شكرا لامانتك" : "The application commission has been paid successfully. Thank you for your honesty."
                    };
                }
                else
                {
                    return new BaseResponseDto<string>
                    {
                        Message = lang == "ar" ? "عفوا لا يوجد لديك رصيد كافي بالمحفظه" : "Sorry, you do not have enough balance in your wallet."
                    };
                }
            }
            else
            {
                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, Check Data"
                };
            };

        }

        public async Task<BaseResponseDto<string>> Policy()
        {
            var lang = _langService.Lang;

            var data = _db.Settings.Select(c => lang == "ar" ? c.PolicyAr : c.PolicyEn).FirstOrDefault();
            return new BaseResponseDto<string>
            {
                Data = data,
                IsSuccess = true
            };
        }

        public async Task<BaseResponseDto<string>> DiscountSystem()
        {
            var lang = _langService.Lang;

            var data = _db.Settings.Select(c => lang == "ar" ? c.DiscountSystemAr : c.DiscountSystemEn).FirstOrDefault();
            return new BaseResponseDto<string>
            {
                Data = data,
                IsSuccess = true
            };
        }

        public async Task<BaseResponseDto<string>> TermsAndConditions()
        {
            var lang = _langService.Lang;

            var data = _db.Settings.Select(c => lang == "ar" ? c.CondtionsArClient : c.CondtionsEnClient).FirstOrDefault();
            return new BaseResponseDto<string>
            {
                Data = data,
                IsSuccess = true
            };
        }

        public async Task<BaseResponseDto<ContactUsInfoDto>> ContactWithUs()
        {
            var lang = _langService.Lang;

            var data = _db.Settings.Select(c => new ContactUsInfoDto
            {
                contactUsText = lang == "ar" ? c.ContactUsAr : c.ContactUsEn,
                email = c.Email,
                location = c.Address,
                lat = c.Lat,
                lng = c.Lng,
                phone = c.Phone
            }).FirstOrDefault();

            return new BaseResponseDto<ContactUsInfoDto>
            {
                Data = data,
                IsSuccess = true
            };
        }

        public async Task<BaseResponseDto<List<ProhibitedGoodsListDto>>> ProhibitedGoods()
        {
            var lang = _langService.Lang;
            var prohibitedGoods = _db.ProhibitedGoods.Where(d => d.IsActive).Select(c => new ProhibitedGoodsListDto
            {
                id = c.Id,
                name = lang == "ar" ? c.NameAr : c.NameEn,
                description = lang == "ar" ? c.DescriptionAr : c.DescriptionEn,
            }).OrderByDescending(o => o.id).ToList();

            return new BaseResponseDto<List<ProhibitedGoodsListDto>>
            {
                Data = prohibitedGoods,
                IsSuccess = true
            };

        }

        public async Task<BaseResponseDto<string>> AddNewComplaint(AddNewComplaintsDto model)
        {
            var lang = _langService.Lang;

            try
            {
                Complaints complaint = new Complaints
                {
                    Name = model.name,
                    Message = model.message,
                    PhoneCode = model.phoneCode,
                    Phone = model.phone,
                    UserId = _currentUserService.UserId,
                    CreationDate = HelperDate.GetCurrentDate(),
                    IsReplay = false,
                    CodeComplaint = RandomString(6)
                };

                _db.Complaints.Add(complaint);
                _db.SaveChanges();

                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                    Message = lang == "ar" ? "لقد تم إرسال الشكوى إلى الإدارة بنجاح، وسيتم الرد قريبًا" : "The complaint has been sent to the management successfully, and a response will be provided soon."
                };

            }
            catch (Exception ex)
            {

                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا تأكد من البيانات" : "Please, Check Data"
                };
            }
        }

        public async Task<BaseResponseDto<List<ComplaintsListDto>>> ComplaintsList(int type)
        {
            var lang = _langService.Lang;

            var complaints = _db.Complaints.Where(d => d.UserId == _currentUserService.UserId);

            if (type == (int)TypeComplaint.Waiting)
            {
                complaints = complaints.Where(d => !d.IsReplay);
            }
            else
            {
                complaints = complaints.Where(d => d.IsReplay);
            }

            var result = complaints.Select(c => new ComplaintsListDto
            {
                codeComplaint = c.CodeComplaint,
                complaintId = c.Id,
                date = HelperDate.FormatDate(c.CreationDate, lang),
                status = !c.IsReplay ? (lang == "ar" ? "لم يتم الرد" : "No response") : (lang == "ar" ? "تم الرد" : "Reply has been made")
            }).OrderByDescending(o => o.complaintId).ToList();

            return new BaseResponseDto<List<ComplaintsListDto>>
            {
                Data = result,
                IsSuccess = true
            };
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@$#!%*";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<BaseResponseDto<ComplaintsDetailsDto>> ComplaintInfo(int complaintId)
        {
            var lang = _langService.Lang;

            var complaint = _db.Complaints.Where(d => d.Id == complaintId).Select(c => new ComplaintsDetailsDto
            {
                complaintId = c.Id,
                codeComplaint = c.CodeComplaint,
                date = HelperDate.FormatDate(c.CreationDate, lang),
                message = c.Message,
                replay = c.Replay,
                status = !c.IsReplay ? (lang == "ar" ? "قيد المراجعه" : "Under review") : (lang == "ar" ? "تم الرد" : "Reply has been made"),
                userName = c.EmployeeId != null ? c.Employee.user_Name : null,
                userImage = c.EmployeeId != null ? DashBordUrl.baseUrlHost + c.Employee.ImgProfile : null

            }).FirstOrDefault();

            return new BaseResponseDto<ComplaintsDetailsDto>
            {
                Data = complaint,
                IsSuccess = true
            };
        }

        public async Task<BaseResponseDto<string>> Footer()
        {
            var lang = _langService.Lang;
            var foot = _db.Settings.Select(c => lang == "ar" ? c.FooterAr : c.FooterEn).FirstOrDefault();
            return new BaseResponseDto<string>
            {
                Data = foot,
                IsSuccess = true
            };
        }
    }
}
