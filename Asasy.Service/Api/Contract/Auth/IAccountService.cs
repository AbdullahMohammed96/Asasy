using Asasy.Domain.DTO;
using Asasy.Domain.DTO.AuthApiDTO;
using Asasy.Domain.DTO.AuthDTO;
using Asasy.Domain.Entities.UserTables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Contract.Auth
{
    public interface IAccountService
    {
        Task<int> GenerateCode(int currentCode);
        Task<bool> AddDeviceId(DeviceIdAddDto deviceIdAddDto);
        Task<string> SendSms(int code, string phone);
        Task<ApplicationDbUser> AddUser(UserInfoAddDTO userInfoAddDTO);
        Task<bool> AddUserToRole(ApplicationDbUser userInfoAddDTO, string roleName);
        Task<ConfirmCodeDto> ConfirmCodeRegister(ConfirmCodeAddDto confirmCodeAddDto);
        Task<ResendCodeDto> ResendCode(ResendCodeAddDto resendCodeAddDto);
        Task<bool> CheckPassword(ApplicationDbUser user, string password);
        Task<bool> ValidatePassword(string password, string showPassword);
        Task<bool> CheckAccountBlocked(ApplicationDbUser user);
        Task<bool> CheckIsActiveOrNo(ApplicationDbUser user);
        Task<bool> ChangeLanguage(string lang, string userId);
        Task<RemoveAccountDto> RemoveAccount(string userId, string lang);
        Task<ApplicationDbUser> GetUserUsingPhone(string PhoneNumber,string phoneCode);
        Task<ApplicationDbUser> GetUserUsingId(string UserId);
        Task<bool> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<bool> UpdateCode(string UserPhone,string phoneCode, int Code);
        Task<bool> ChangePasswordByCode(ChangePasswordByCodeDto changePasswordByCodeDto);
        Task<bool> Logout(LogoutDto userModel);



        Task<bool> UpdateCodeUsingUserId(string UserPhone, int Code, string UserId);
        Task<bool> CheckCodeIsCorrect(int code);
        Task<bool> UpdatePhoneNumber(string phoneNumber, string phoneCode);

        Task<BaseResponseDto<string>> ChangeStatusForActiveOnApp();


    }
}
