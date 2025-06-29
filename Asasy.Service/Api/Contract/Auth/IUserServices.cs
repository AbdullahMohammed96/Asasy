using Asasy.Domain.DTO;
using Asasy.Domain.DTO.AuthApiDTO;
using Asasy.Domain.DTO.AuthDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Contract.Auth
{
    public interface IUserServices
    {
        Task<bool> DeleteAllNotify();
        Task<bool> ChangeNotify(ChangeNotifyEditDto changeNotifyEditDto);
        Task<bool> AddUserNotify(string textAr, string textEn, string userId, int stutes, int orderId = 0);
        Task<bool> SendNotify(string textAr, string textEn, string fkProvider, int stutes, int orderId = 0);
        Task<bool> SendNotifyAsync(string textAr, string textEn, string userId, int stutes);
        Task<List<NotifyListDto>> ListOfNotifyUser(string userId, string lang = "ar");
        Task<BaseResponseDto<string>> RemoveNotifyById(int notifyId);
        Task<BaseResponseDto<string>> CountNotifactions();
        Task<bool> CheckValidatePhone(string phone);
        Task<bool> CheckValidateEmail(string email);
        Task<UserInfoDTO> GetUserInfo(string userId, string lang, string token);
        Task<UserInfoDTO> GetProviderInfo(string userId, string lang, string token);
        Task<bool> UpdateDataUser(UpdateDataUserDto updateDataUserDto);


        Task<bool> CheckValidatePhoneUsingUserId(string phone, string userId);


    }
}
