using AAITHelper.ModelHelper;
using AAITHelper;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.DTO.AuthDTO;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Auth;
using Asasy.Service.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAITHelper.Enums;
using AutoMapper.QueryableExtensions;
using Asasy.Domain.DTO.AuthApiDTO;
using Asasy.Domain.DTO;
using Asasy.Service.Api.Contract.Lang;

namespace Asasy.Service.Api.Implementation.Auth
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHelper _uploadImage;
        private readonly ILangService _langService;


        public UserServices(ApplicationDbContext db, UserManager<ApplicationDbUser> userManager, ICurrentUserService currentUserService, IHelper uploadImage = null, ILangService langService = null)
        {
            this.db = db;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _uploadImage = uploadImage;
            _langService = langService;
        }

        public async Task<bool> DeleteAllNotify()
        {
            var currentUserId = _currentUserService.UserId;
            var userNotifications = db.NotifyUsers.Where(n => n.UserId == currentUserId);
            db.NotifyUsers.RemoveRange(userNotifications);
            await db.SaveChangesAsync();

            return true;
        }
        public async Task<bool> ChangeNotify(ChangeNotifyEditDto changeNotifyEditDto)
        {
            ApplicationDbUser foundeUser = await db.Users.FindAsync(changeNotifyEditDto.userId);
            foundeUser.CloseNotify = changeNotifyEditDto.notify;
            await db.SaveChangesAsync();
            return true;

        }
        public async Task<bool> AddUserNotify(string textAr, string textEn, string userId, int stutes, int orderId = 0)
        {
            try
            {
                await db.NotifyUsers.AddAsync(new()
                {
                    Date = HelperDate.GetCurrentDate(),
                    UserId = userId,
                    Show = false,
                    TextAr = textAr,
                    TextEn = textEn,
                    Type = stutes,
                    AdsId = orderId

                });
                await db.HistoryNotify.AddAsync(new()
                {
                    Text = textAr,
                    Date = HelperDate.GetCurrentDate(),
                });
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public async Task<bool> SendNotifyAsync(string textAr, string textEn, string userId, int stutes)
        {
            try
            {
                var user = await db.Users.FindAsync(userId);

                await db.NotifyUsers.AddAsync(new()
                {
                    Date = HelperDate.GetCurrentDate(),
                    UserId = userId,
                    Show = false,
                    TextAr = textAr,
                    TextEn = textEn,
                    Type = stutes
                });
                await db.SaveChangesAsync();

                var setting = await db.Settings.FirstOrDefaultAsync();

                List<DeviceIdModel> AllDeviceids = await db.DeviceIds.Where(x => x.UserId == userId).Select(x => new DeviceIdModel()
                {
                    DeviceId = x.DeviceId_,
                    DeviceType = x.DeviceType,
                    FkUser = x.UserId,
                    ProjectName = x.ProjectName

                }).ToListAsync();
                HelperFcm.SendPushNotification(setting.ApplicationId, setting.SenderId, AllDeviceids, null, textAr, stutes);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> SendNotify(string textAr, string textEn, string fkProvider, int stutes, int orderId = 0)
        {
            ApplicationDbUser user = await _userManager.FindByIdAsync(fkProvider);
            Setting Fcm = await db.Settings.FirstOrDefaultAsync();
            List<DeviceIdModel> AllDeviceids = await db.DeviceIds.Where(x => x.UserId == fkProvider).Select(x => new DeviceIdModel()
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName

            }).AsNoTracking().ToListAsync();
            HelperFcm.SendPushNotification(Fcm.ApplicationId, Fcm.SenderId, AllDeviceids, null, textAr, stutes, user.TypeUser, orderId);
            return true;
        }
        public async Task<List<NotifyListDto>> ListOfNotifyUser(string userId, string lang = "ar")
        {
            try
            {
                List<NotifyListDto> Notify = await db.NotifyUsers.Where(x => x.UserId == userId).Select(x => new NotifyListDto
                {
                    id = x.Id,
                    text = HelperMsg.creatMessage(lang, x.TextAr, x.TextEn),
                    date = x.Date.ToString("dd/MM/yyyy h:mm tt"),
                    type = x.Type,
                    adsId = x.AdsId

                }).OrderByDescending(x => x.id).ToListAsync();
                List<NotifyUser> updateNotfy = await db.NotifyUsers.Where(x => x.Show == false && x.UserId == userId).ToListAsync();
                updateNotfy.ForEach(a => a.Show = true);
                await db.SaveChangesAsync();
                return new List<NotifyListDto>(Notify);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> CheckValidateEmail(string email)
        {

            ApplicationDbUser foundedEmail = await _userManager.FindByEmailAsync(email);
            return foundedEmail != null;
        }
        public async Task<bool> CheckValidatePhone(string phone)
        {
            ApplicationDbUser foundedPhone = await db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phone);
            return foundedPhone != null;
        }
        public async Task<UserInfoDTO> GetUserInfo(string userId, string lang, string token)
        {
            UserInfoDTO userInfoDTO = await db.Users.Where(x => x.Id == userId && x.TypeUser == UserType.Client.ToNumber())
                                                    .AsQueryable()
                                                    .AsNoTracking()
                                                    .ProjectTo<UserInfoDTO>(MappingProfiles.UserInfo(token))
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync();
            return userInfoDTO;
        }
        public async Task<UserInfoDTO> GetProviderInfo(string userId, string lang, string token)
        {
            UserInfoDTO userInfoDTO = await db.Users.Where(x => x.Id == userId && x.TypeUser == UserType.deleget.ToNumber())
                                                    .AsQueryable()
                                                    .AsNoTracking()
                                                    .ProjectTo<UserInfoDTO>(MappingProfiles.ProviderInfo(token))
                                                    .FirstOrDefaultAsync();
            return userInfoDTO;
        }
        public async Task<bool> UpdateDataUser(UpdateDataUserDto userModel)
        {
            try
            {
                var user = (await db.Users.Where(x => x.Id == _currentUserService.UserId).FirstOrDefaultAsync());

                user.user_Name = userModel.userName ?? user.user_Name;
                user.Email = userModel.email ?? user.Email;

                if (userModel.imgProfile != null)
                {
                    user.ImgProfile = _uploadImage.Upload(userModel.imgProfile, (int)FileName.Users);
                }
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public async Task<bool> CheckValidatePhoneUsingUserId(string phone, string userId)
        {
            ApplicationDbUser foundedPhone = await db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phone && x.Id != userId);
            return foundedPhone != null;
        }

        public async Task<BaseResponseDto<string>> RemoveNotifyById(int notifyId)
        {
            var lang = _langService.Lang;
            var notify = db.NotifyUsers.Where(d => d.Id == notifyId).FirstOrDefault();
            if (notify != null)
            {
                db.NotifyUsers.Remove(notify);
                db.SaveChanges();

                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "تم الحذف بنجاح" : "Deleted successfully",
                    IsSuccess = true
                };
            }
            else
            {
                return new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "حدث خطأ ما فضلا تأكد من البيانات" : "Something went wrong, please check the data."
                };
            }
        }

        public async Task<BaseResponseDto<string>> CountNotifactions()
        {
            var countNotifactionNotShow = db.NotifyUsers.Where(d => d.UserId == _currentUserService.UserId && !d.Show).Count();

            return new BaseResponseDto<string>
            {
                IsSuccess = true,
                Data = countNotifactionNotShow.ToString()
            };
        }
    }
}
