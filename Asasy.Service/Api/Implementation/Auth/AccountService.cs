using AAITHelper;
using AAITHelper.ModelHelper;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.DTO;
using Asasy.Domain.DTO.AuthApiDTO;
using Asasy.Domain.DTO.AuthDTO;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Auth;
using Base.Domain.Common.Helpers;
using Base.Domain.Common.Helpers._4jawaly.Models._4jawalyProvider;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Asasy.Service.Api.Implementation.Auth
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHelper uploadImage;
        public AccountService(ApplicationDbContext db, UserManager<ApplicationDbUser> userManager, ICurrentUserService currentUserService, IHelper uploadImage = null)
        {
            this.db = db;
            _userManager = userManager;
            _currentUserService = currentUserService;
            this.uploadImage = uploadImage;
        }

        public async Task<bool> AddDeviceId(DeviceIdAddDto deviceIdAddDto)
        {
            try
            {
                bool check_device_id = await db.DeviceIds.AnyAsync(st => st.DeviceId_ == deviceIdAddDto.deviceId && st.UserId == deviceIdAddDto.userId);
                if (!check_device_id)
                {

                    await db.DeviceIds.AddAsync(new()
                    {
                        DeviceId_ = deviceIdAddDto.deviceId,
                        UserId = deviceIdAddDto.userId,
                        DeviceType = deviceIdAddDto.deviceType,
                        ProjectName = deviceIdAddDto.projectName
                    });
                }
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<ApplicationDbUser> AddUser(UserInfoAddDTO userInfoAddDTO)
        {
            try
            {
                ApplicationDbUser user = new ApplicationDbUser
                {
                    Email = userInfoAddDTO.email ?? "",
                    UserName = userInfoAddDTO.phone + HelperNumber.GetRandomNumber() + "@yahoo.com",
                    user_Name = userInfoAddDTO.userName,
                    ShowPassword = "",
                    ImgProfile = uploadImage.Upload(userInfoAddDTO.imgProfile, (int)FileName.Users),
                    PhoneCode = userInfoAddDTO.phoneCode,
                    Wallet = 0,
                    IsActive = true,
                    ActiveCode = false,
                    CloseNotify = false,
                    LastAppearance = HelperDate.GetCurrentDate(),
                    PublishDate = HelperDate.GetCurrentDate(),
                    Code = userInfoAddDTO.code,
                    PhoneNumber = userInfoAddDTO.phone,
                    TypeUser = (int)UserType.Client,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Lang = userInfoAddDTO.lang,
                };
                _ = await _userManager.CreateAsync(user, "123456");
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<bool> AddUserToRole(ApplicationDbUser userInfoAddDTO, string roleName)
        {
            bool result = (await _userManager.AddToRoleAsync(userInfoAddDTO, roleName)).Succeeded;
            return result;
        }

        public async Task<bool> ChangeLanguage(string lang, string userId)
        {
            ApplicationDbUser Users = await db.Users.FindAsync(userId);
            if (Users != null)
            {
                Users.Lang = lang;
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckAccountBlocked(ApplicationDbUser user)
        {

            return user.IsActive;
        }

        public async Task<bool> CheckIsActiveOrNo(ApplicationDbUser user)
        {
            return user.ActiveCode;
        }

        public async Task<bool> CheckPassword(ApplicationDbUser user, string password)
        {
            bool isCorrect = await _userManager.CheckPasswordAsync(user, password);
            return isCorrect;
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

        public async Task<ConfirmCodeDto> ConfirmCodeRegister(ConfirmCodeAddDto confirmCodeAddDto)
        {

            ApplicationDbUser foundedUser = await _userManager.FindByIdAsync(confirmCodeAddDto.userId);
            if (foundedUser != null)
            {
                if (foundedUser.Code == confirmCodeAddDto.code)
                {
                    foundedUser.ActiveCode = true;
                    foundedUser.IsOnline = true;
                    await db.SaveChangesAsync();
                    return new ConfirmCodeDto { userId = foundedUser.Id, typeUser = foundedUser.TypeUser };
                }
                else
                {
                    return new ConfirmCodeDto { userId = "", typeUser = 0 };
                }

            }
            else
            {
                return new ConfirmCodeDto { userId = "", typeUser = 0 };
            }
        }

        public async Task<int> GenerateCode(int currentCode)
        {
            try
            {
                int code = HelperNumber.GetRandomNumber(currentCode);
                Setting GetInfoSms = await db.Settings.FirstOrDefaultAsync();
                if (GetInfoSms != null)
                {
                    if (GetInfoSms.SenderName != "test")
                    {
                        code = HelperNumber.GetRandomNumber();
                    }
                }
                return code;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public async Task<ResendCodeDto> ResendCode(ResendCodeAddDto resendCodeAddDto)
        {
            ApplicationDbUser foundedUser = await _userManager.FindByIdAsync(resendCodeAddDto.userId);
            if (foundedUser != null)
            {
                if (foundedUser.PhoneNumber != "546196863")
                {
                    int code = await GenerateCode(1234);
                    _ = await SendSms(code, foundedUser.PhoneNumber);
                    foundedUser.Code = code;
                    await db.SaveChangesAsync();
                    return new ResendCodeDto { code = code, phone = foundedUser.PhoneNumber, userId = foundedUser.Id };
                }
                return new ResendCodeDto { code = 1234, phone = foundedUser.PhoneNumber, userId = foundedUser.Id };

            }
            else
            {
                return new ResendCodeDto { userId = "", phone = "", code = 0 };
            }
        }

        public async Task<string> SendSms(int code, string phone)
        {
            //Setting GetInfoSms = await db.Settings.FirstOrDefaultAsync();
            //if (GetInfoSms != null)
            //{
            //    if (GetInfoSms.SenderName != "test")
            //    {
            //        string resultSms = await HelperSms.SendMessageBy4jawaly(code.ToString(), phone, GetInfoSms.SenderName, GetInfoSms.UserNameSms, GetInfoSms.PasswordSms);
            //        return resultSms;
            //    }
            //}

            Setting GetInfoSms = await db.Settings.FirstOrDefaultAsync();
            if (GetInfoSms != null)
            {
                string autoFillCode = "BpoQI2VpPtL";
                var text = $"Your verification code is: {code} {autoFillCode}";
                if (GetInfoSms.SenderName != "test")
                {

                    var model = new Root()
                    {
                        numbers = phone,
                        text = text,
                        app_key = GetInfoSms.UserNameSms,
                        app_secret = GetInfoSms.PasswordSms,
                        sender = GetInfoSms.SenderName
                    };
                    var result = await _4jawalySmsHelper.SendSms(model);
                    return result.message;
                }
            }


            return "";
        }

        public async Task<bool> ValidatePassword(string password, string showPassword)
        {
            return password != showPassword;
        }
        public async Task<RemoveAccountDto> RemoveAccount(string currentUserId, string lang = "ar")
        {

            var foundedUser = await db.Users.FindAsync(currentUserId);
            if (foundedUser == null)
            {

                return new RemoveAccountDto
                {
                    check = false,
                    message = lang == "ar" ? "المستخدم غير موجود" : "User not found"
                };
            }

            if (foundedUser.Wallet > 0)
            {
                return new RemoveAccountDto
                {
                    check = false,
                    message = lang == "ar" ? "يوجد رصيد بالمحفظه لا يمكنك حذف الحساب" : "There is a balance in the wallet. You cannot delete the account."
                };
            }

            foundedUser.user_Name = foundedUser.user_Name + Guid.NewGuid().ToString();
            foundedUser.PhoneNumber = foundedUser.PhoneNumber + Guid.NewGuid().ToString();
            foundedUser.Email = foundedUser.Email + Guid.NewGuid().ToString();
            foundedUser.NormalizedEmail = foundedUser.NormalizedEmail + Guid.NewGuid().ToString();
            foundedUser.UserName = foundedUser.UserName + Guid.NewGuid().ToString();
            foundedUser.NormalizedUserName = foundedUser.NormalizedUserName + Guid.NewGuid().ToString();
            foundedUser.IsDeleted = true;

            var adsForUser = db.AdvertsmentDetails.Where(d => d.UserId == currentUserId).ToList();
            adsForUser.ForEach(a => a.IsDelete = true);

            var parentDeviceIds = await db.DeviceIds.Where(x => x.UserId == currentUserId).Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.ProjectName,
                ProjectName = x.ProjectName
            })
           .ToListAsync();

            //int type = -3;
            //Dictionary<string, string> data = new Dictionary<string, string>();
            //data.Add("info", currentUserId);
            //data.Add("type", type.ToString());


            //NotificationHelper
            //    .SendPushNewNotificationAsync(parentDeviceIds, data, foundedUser.Lang == "ar" ? "تم حذف الحساب بنجاح" : "Account deleted successfully");


            var deviceIds = db.DeviceIds.Where(d => d.UserId == currentUserId).ToList();
            db.DeviceIds.RemoveRange(deviceIds);
            await db.SaveChangesAsync();

            return new RemoveAccountDto
            {
                check = true,
                message = lang == "ar" ? "تم حذف الحساب بنجاح" : "The account was deleted successfully"
            };
        }

        public async Task<ApplicationDbUser> GetUserUsingPhone(string PhoneNumber, string phoneCode)
        {
            string englishPhoneNumber = HelperNumber.ConvertArabicNumberToEnglish(PhoneNumber);
            return await db.Users.Where(x => x.PhoneNumber == englishPhoneNumber).SingleOrDefaultAsync();
        }
        public async Task<ApplicationDbUser> GetUserUsingId(string UserId)
        {
            return await db.Users.SingleOrDefaultAsync(x => x.Id == UserId);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var user = (await _userManager.FindByIdAsync(_currentUserService.UserId));

            if (await ValidatePassword(user.ShowPassword, changePasswordDto.oldPassword))
                return false;

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordDto.oldPassword, changePasswordDto.newPassword);
            if (!changePasswordResult.Succeeded)
                return false;

            user.ShowPassword = changePasswordDto.newPassword;
            return await db.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateCode(string UserPhone, string phoneCode, int Code)
        {
            var user = await GetUserUsingPhone(UserPhone, phoneCode);
            user.Code = Code;
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangePasswordByCode(ChangePasswordByCodeDto changePasswordByCodeDto)
        {
            var codeuser = await GetUserUsingId(changePasswordByCodeDto.userId);

            var changePasswordResult = await _userManager.ChangePasswordAsync(codeuser, codeuser.ShowPassword, changePasswordByCodeDto.newPassword);
            if (!changePasswordResult.Succeeded)
            {
                return false;
            }
            codeuser.ShowPassword = changePasswordByCodeDto.newPassword;
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Logout(LogoutDto userModel)
        {
            var info = await db.DeviceIds.Where(st => st.DeviceId_ == userModel.deviceId && st.UserId == _currentUserService.UserId).ToListAsync();
            if (info.Count > 0)
            {
                foreach (var item in info)
                {
                    db.DeviceIds.Remove(item);
                    await db.SaveChangesAsync();
                }
                return true;
            }
            else
            {
                return true;
            }

        }



        public async Task<bool> UpdateCodeUsingUserId(string UserPhone, int Code, string UserId)
        {
            var user = await GetUserUsingId(UserId);
            user.Code = Code;
            //user.ActiveCode = false;
            await db.SaveChangesAsync();
            return true;
        }


        public async Task<bool> CheckCodeIsCorrect(int code)
        {
            var user = db.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();
            if (user != null && user.Code == code)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        public async Task<bool> UpdatePhoneNumber(string phoneNumber, string phoneCode)
        {
            try
            {
                var user = db.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();
                if (user != null)
                {
                    user.PhoneNumber = phoneNumber ?? user.PhoneNumber;
                    user.PhoneCode = phoneCode ?? user.PhoneCode;
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<BaseResponseDto<string>> ChangeStatusForActiveOnApp()
        {

            var user = db.Users.Where(d => d.Id == _currentUserService.UserId).FirstOrDefault();
            if (user != null)
            {
                user.IsOnline = !user.IsOnline;
                user.LastAppearance = HelperDate.GetCurrentDate();

                db.SaveChanges();

                return new BaseResponseDto<string>
                {
                    IsSuccess = true,
                };
            }
            else
            {
                return new BaseResponseDto<string>
                {
                    IsSuccess = false,
                };
            }

        }
    }
}
