using AAITHelper.Enums;
using AAITHelper;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.DTO.AuthApiDTO;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Asasy.Domain.DTO.AuthDTO;
using Microsoft.EntityFrameworkCore;
using Asasy.Service.Api.Contract.Lang;
using Asasy.Domain.DTO;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using Asasy.Service.Api.Implementation.Auth;
using Asasy.Service.Api.Implementation.FilterValidation;

namespace Asasy.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "UserAPI")]
    [PermessionFilterAttribute]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserServices _userService;
        private readonly ILangService _langService;
        private readonly IAccountService _accountService;


        public UserController(ICurrentUserService currentUserService, IUserServices userServices, ILangService langService, IAccountService accountService)
        {
            _currentUserService = currentUserService;
            _userService = userServices;
            _langService = langService;
            _accountService = accountService;
        }

        /// <summary>
        /// get user data
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<UserInfoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Identity.GetDataOfUser)]
        public async Task<IActionResult> GetDataOfUser()
        {
            string lang = _langService.Lang;
            try
            {
                var response = await _userService.GetUserInfo(_currentUserService.UserId, lang, "");
                if (response is not null)
                    return Ok(new BaseResponseDto<UserInfoDTO> { Data = response, IsSuccess = true });

                else
                {
                    return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.creatMessage(lang, "لم يتم العثور علي هذا المستخدم", "Not Found") });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), lang) });
            }
        }


        /// <summary>
        /// update data for user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// 
        /// 
        ///                 {
        ///                 "userName": "abdelrahman_elwsef",
        ///                 "email": "abdelrahman12@gmail.com",
        ///                 "imgProfile": file,
        ///                  }
        /// </remarks>
        [HttpPut(ApiRoutes.Identity.UpdateDataUser)]
        public async Task<IActionResult> UpdateDataUser([FromForm] UpdateDataUserDto userModel)
        {
            string lang = _langService.Lang;


            if (userModel.email != null)
            {
                var emailExist = await _userService.CheckValidateEmail(userModel.email);
                if (emailExist)
                {
                    return BadRequest(HelperMsg.MsgValidation(EnumValidMsg.Auth.EmailExisting.ToNumber(), lang));
                }
            }

            try
            {
                var check = await _userService.UpdateDataUser(userModel);
                if (check)
                {
                    return Ok(new BaseResponseDto<UserInfoDTO> { Data = await _userService.GetUserInfo(_currentUserService.UserId, lang, ""), IsSuccess = true, Message = lang == "ar" ? "تم تحديث البيانات بنجاح" : "Data updated successfully" });
                }
                else
                {
                    return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), lang) });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber(), lang));
            }
        }


        /// <summary>
        /// send otp to phone number; first sende get old phone number from user model .. second sende for new phone number from client
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// 
        /// 
        ///                 {
        ///                 "userName": "abdelrahman_elwsef",
        ///                 "email": "abdelrahman12@gmail.com",
        ///                 "imgProfile": file,
        ///                  }
        /// </remarks>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Identity.SendCodeToUpdate)]
        public async Task<IActionResult> SendCodeToUpdate([FromForm] SendCodeToPhoneDto model)
        {
            string lang = _langService.Lang;


            if (model.phoneNumber == null)
            {
                return BadRequest(new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا قم بادخال رقم الجوال" : "Please enter your mobile number"
                });
            }

            var userId = _currentUserService.UserId;
            var checkPhone = await _userService.CheckValidatePhoneUsingUserId(model.phoneNumber, userId);
            if (checkPhone)
            {
                return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.PhoneExisting.ToNumber(), lang), IsSuccess = false });
            }

            int code = await _accountService.GenerateCode(1234);

            // this service beacuse I use This service 
            await _accountService.UpdateCodeUsingUserId(model.phoneNumber, code, userId);

            _ = await _accountService.SendSms(code, model.phoneNumber);

            return Ok(new BaseResponseDto<string> { IsSuccess = true, Message = HelperMsg.creatMessage(lang, "تم ارسال كود التفعيل علي رقم الجوال بنجاح من فضلك قم بالتفعيل", "The activation code has been sent to your mobile number successfully. Please activate") });

        }

        /// <summary>
        /// use this service for first check code when send otp to old phone
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// 
        /// 
        ///                 {
        ///                 "code": "1234
        ///                  }
        /// </remarks>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Identity.CheckCodeIsCorrectOrNot)]
        public async Task<IActionResult> CheckCodeIsCorrectOrNot(int code)
        {
            string lang = _langService.Lang;

            var check = await _accountService.CheckCodeIsCorrect(code);
            if (check)
            {
                return Ok(new BaseResponseDto<string> { IsSuccess = true, Message = lang == "ar" ? "تم تفعيل الكود بنجاح" : "The code has been activated successfully." });
            }
            else
            {
                return Ok(new BaseResponseDto<string> { IsSuccess = false, Message = lang == "ar" ? "عفوا الكود غير صحيح فضلا تأكد من البيانات" : "Sorry, the code is incorrect. Please check the data" });

            }
        }



        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Identity.ChangeAppearanceStatus)]
        public async Task<IActionResult> ChangeAppearanceStatus()
        {

            var check = await _accountService.ChangeStatusForActiveOnApp();
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);

            }
        }

        /// <summary>
        /// use this service when confirm code for new phone number and change phone 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// 
        /// 
        ///                 {
        ///                 "phoneNumber": "501231231
        ///                 "phoneCode": "1234
        ///                 "code": "1234
        ///                  }
        /// </remarks>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPut(ApiRoutes.Identity.ChangePhoneNumberByCode)]
        public async Task<IActionResult> ChangePhoneNumberByCode([FromForm] ChangePhoneNumberByCodeDto model)
        {
            string lang = _langService.Lang;

            if (model.phoneNumber == null)
            {
                return BadRequest(new BaseResponseDto<string>
                {
                    Message = lang == "ar" ? "فضلا قم بادخال رقم الجوال" : "Please enter your mobile number"
                });
            }


            var check = await _accountService.CheckCodeIsCorrect(model.code);
            if (check)
            {
                if (await _accountService.UpdatePhoneNumber(model.phoneNumber, model.phoneCode))
                {
                    return Ok(new BaseResponseDto<string> { IsSuccess = true, Message = lang == "ar" ? "تم تغيير رقم الجوال بنجاح" : "The mobile number has been changed successfully" });
                }
                else
                {
                    return Ok(new BaseResponseDto<string> { IsSuccess = false, Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), lang) });
                }
            }
            else
            {
                return Ok(new BaseResponseDto<string> { IsSuccess = false, Message = lang == "ar" ? "عفوا الكود غير صحيح فضلا تأكد من البيانات" : "Sorry, the code is incorrect. Please check the data" });
            }

        }


        /// <summary>
        /// Remove All Notify
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Identity.RemoveAllNotify)]
        public async Task<IActionResult> RemoveAllNotify()
        {
            string lang = _langService.Lang;
            if (await _userService.DeleteAllNotify())
            {
                return Ok(new BaseResponseDto<string> { Message = lang == "ar" ? "تم الحذف بنجاح" : "Deleted successfully", IsSuccess = true });
            }
            else
            {
                return Ok(new BaseResponseDto<string> { IsSuccess = false, Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), lang) });
            }

        }


        /// <summary>
        /// Close Notification for users
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPatch(ApiRoutes.Identity.ChangeNotify)]
        public async Task<IActionResult> ChangeNotify(bool notify)
        {
            string lang = _langService.Lang;

            if (await _userService.ChangeNotify(new ChangeNotifyEditDto()
            {
                notify = notify,
                userId = _currentUserService.UserId,
                lang = lang
            }))
            {
                return Ok(new BaseResponseDto<string> { IsSuccess = true, Message = lang == "ar" ? "تم التغيير بنجاح" : "Changed successfully" });
            }
            else
            {
                return Ok(new BaseResponseDto<string> { IsSuccess = false, Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), lang) });
            }

        }

        /// <summary>
        /// List Notification for Client
        /// </summary>
        /// <remarks>
        ///Sample response:
        ///
        /// 
        /// 
        /// 
        ///               {
        ///              "id": "1",
        ///              "text": any,
        ///              "date": 18/10/2021,
        ///              "type":1,
        ///               }
        /// </remarks>
        [ProducesResponseType(typeof(BaseResponseDto<List<NotifyListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Identity.ListOfNotifyUser)]
        public async Task<IActionResult> ListOfNotifyUser()
        {
            string lang = _langService.Lang;
            return Ok(new BaseResponseDto<List<NotifyListDto>> { IsSuccess = true, Data = await _userService.ListOfNotifyUser(_currentUserService.UserId, lang) });
        }

        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Identity.CountNotifactions)]
        public async Task<IActionResult> CountNotifactions()
        {
            var data = await _userService.CountNotifactions();
            return Ok(data);
        }
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Identity.RemoveNotify)]
        public async Task<IActionResult> RemoveNotify(int notifyId)
        {
            string lang = _langService.Lang;
            var check = await _userService.RemoveNotifyById(notifyId);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }

    }

}
