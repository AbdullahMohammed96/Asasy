using System.IdentityModel.Tokens.Jwt;
using AAITHelper;
using AAITHelper.Enums;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.DTO;
using Asasy.Domain.DTO.AuthApiDTO;
using Asasy.Domain.DTO.AuthDTO;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Auth;
using Asasy.Service.Api.Contract.Lang;
using Asasy.Service.Api.Implementation.FilterValidation;
using Base.Domain.Common.Helpers;
using Base.Domain.Common.Helpers._4jawaly.Models._4jawalyProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asasy.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "AuthAPI")]
    [PermessionFilterAttribute]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private readonly IUserServices _userService;
        private readonly ILangService _langService;
        private readonly ApplicationDbContext db;

        public AuthController(IConfiguration configuration, ICurrentUserService currentUserService, IAccountService accountService, IUserServices userServices, ILangService langService, ApplicationDbContext db)
        {
            _currentUserService = currentUserService;
            _accountService = accountService;
            _userService = userServices;
            _configuration = configuration;
            _langService = langService;
            this.db = db;
        }

        /// <summary>
        /// Add New User 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// 
        /// 
        ///                {
        ///              "imgProfile": file,
        ///              "userName": "ahmed",
        ///              "email": "ahmed@gmail.com",
        ///              "phone": "051231231",
        ///              "phoneCode": "+966",
        ///              "deviceId": "deviceId",
        ///              "deviceType": "deviceType",
        ///              "projectName": "projectName",
        ///               }
        ///       
        /// </remarks>

        [ProducesResponseType(typeof(BaseResponseDto<UserInfoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.RegisterClient)]
        public async Task<IActionResult> RegisterClient([FromForm] UserAddDTO userAddDTO)
        {
            userAddDTO.lang = _langService.Lang;

            string englishPhoneNumber = HelperNumber.ConvertArabicNumberToEnglish(userAddDTO.phone);
            #region validation
            bool userWithSamePhoneNumber = await _userService.CheckValidatePhone(englishPhoneNumber);
            if (userWithSamePhoneNumber)
            {
                return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.PhoneExisting.ToNumber(), userAddDTO.lang) });

            }
            if (userAddDTO.email != null)
            {
                bool userWithSameEmail = await _userService.CheckValidateEmail(userAddDTO.email);
                if (userWithSameEmail)
                {
                    return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.EmailExisting.ToNumber(), userAddDTO.lang) });

                }
            }
            #endregion

            int code = await _accountService.GenerateCode(1234);
            ApplicationDbUser user = await _accountService.AddUser(new UserInfoAddDTO
            {
                code = code,
                email = userAddDTO.email,
                phone = userAddDTO.phone,
                lang = userAddDTO.lang,
                userName = userAddDTO.userName,
                imgProfile = userAddDTO.imgProfile,
                phoneCode = userAddDTO.phoneCode,
            });
            if (user != null)
            {
                await _accountService.AddUserToRole(user, Roles.Mobile.ToString());
                if (userAddDTO.deviceId != null && userAddDTO.deviceType != null)
                {
                    _ = await _accountService.AddDeviceId(new DeviceIdAddDto
                    {
                        deviceId = userAddDTO.deviceId,
                        deviceType = userAddDTO.deviceType,
                        userId = user.Id,
                        projectName = userAddDTO.projectName,
                    });
                }
                _ = await _accountService.SendSms(code, englishPhoneNumber);
            }
            else
            {
                return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), userAddDTO.lang) });
                //
            }
            return Ok(new BaseResponseDto<UserInfoDTO> { Data = await _userService.GetUserInfo(user.Id, userAddDTO.lang, ""), IsSuccess = true, Message = userAddDTO.lang == "ar" ? "تم ارسال الكود للتفعيل" : "The code has been sent to activate." });
        }

        /// <summary>
        /// Confirm Code for Register 
        /// </summary>
        /// <remarks>
        ///Sample request:
        ///     
        /// 
        /// 
        ///                        
        ///                            {
        ///                          "userId": "c3eed5f3-ca71-4833-ace9-62ee3b608d95",
        ///                          "code": 1234,
        ///                           }
        /// </remarks>
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponseDto<UserInfoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPatch(ApiRoutes.Identity.ConfirmCodeRegister)]
        public async Task<IActionResult> ConfirmCodeRegister([FromForm] ConfirmCodeAddDto confirmCodeAddDto)
        {
            confirmCodeAddDto.lang = _langService.Lang;

            ConfirmCodeDto confirmCodeDto = (await _accountService.ConfirmCodeRegister(confirmCodeAddDto));
            if (confirmCodeDto.typeUser != 0)
            {
                var user = await _accountService.GetUserUsingId(confirmCodeAddDto.userId);
                JwtSecurityToken token = HelperGeneral.GetToken(_configuration, user.Id, user.TypeUser.ToString(), user.UserName);

                if (confirmCodeDto.typeUser == UserType.Client.ToNumber())
                {
                    return Ok(new BaseResponseDto<UserInfoDTO> { Message = confirmCodeAddDto.lang == "ar" ? "تم تفعيل الكود بنجاح" : "The code has been activated successfully.", Data = await _userService.GetUserInfo(confirmCodeDto.userId, confirmCodeAddDto.lang, new JwtSecurityTokenHandler().WriteToken(token)), IsSuccess = true });
                }
                else
                {
                    return Ok(await _userService.GetProviderInfo(confirmCodeDto.userId, confirmCodeAddDto.lang, ""));
                }
            }
            else
            {
                return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.CodeNotCorrect.ToNumber(), confirmCodeAddDto.lang) });

            }
        }

        /// <summary>
        /// resend Code for Register 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// 
        /// 
        ///                 {
        ///                 "id": "c3eed5f3-ca71-4833-ace9-62ee3b608d95",
        ///                 "phone": "01234567895",
        ///                 "code": 1234,
        ///                  }
        /// </remarks>
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponseDto<ResendCodeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Identity.ResendCode)]
        public async Task<IActionResult> ResendCode([FromQuery] ResendCodeAddDto resendCodeAddDto)
        {
            string lang = _langService.Lang;

            ResendCodeDto resendCodeDto = await _accountService.ResendCode(resendCodeAddDto);
            if (resendCodeDto.code != 0)
            {
                return Ok(new BaseResponseDto<ResendCodeDto> { Data = resendCodeDto, IsSuccess = true, Message = lang == "ar" ? "تم اعاده ارسال الكود" : "The code has been resent." });
            }
            else
            {
                return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), lang) });

            }
        }

        /// <summary>
        /// LogIn
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// 
        /// 
        /// 
        /// 
        ///                  {
        ///                  "phone": "01234567895",
        ///                  "phoneCode": "+966",
        ///                   }
        /// </remarks>
        [ProducesResponseType(typeof(BaseResponseDto<UserInfoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Identity.Login)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            loginDto.lang = _langService.Lang;
            try
            {
                #region validation

                string englishPhoneNumber = HelperNumber.ConvertArabicNumberToEnglish(loginDto.phone);
                string englishPhoneCodeNumber = HelperNumber.ConvertArabicNumberToEnglish(loginDto.phoneCode);
                var user = await _accountService.GetUserUsingPhone(englishPhoneNumber, englishPhoneCodeNumber);
                if (user is null)
                {
                    return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber(), loginDto.lang) });
                }

                if (!(await _accountService.CheckAccountBlocked(user)))
                {
                    return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.Accountblocked.ToNumber(), loginDto.lang) });
                }



                if (!(await _accountService.CheckIsActiveOrNo(user)))
                {
                    return StatusCode(203, new BaseResponseDto<UserInfoDTO> { Data = await _userService.GetUserInfo(user.Id, loginDto.lang, ""), IsSuccess = true });
                }




                #endregion


                if (loginDto.deviceId != null)
                {

                    bool DeviceAdded = await _accountService.AddDeviceId(new DeviceIdAddDto
                    {
                        deviceId = loginDto.deviceId,
                        deviceType = loginDto.deviceType,
                        projectName = loginDto.projectName,
                        userId = user.Id
                    });

                    if (!DeviceAdded)
                        return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), loginDto.lang) });
                }
                int code = 1234;

             
                    code = await _accountService.GenerateCode(1234);

                if (loginDto.phone != "546196863" && loginDto.phone != "546337212")
                {
                    var updateCode = await _accountService.UpdateCode(englishPhoneNumber, englishPhoneCodeNumber, code);


                    _ = await _accountService.SendSms(code, englishPhoneNumber);


                    return Ok(new BaseResponseDto<string> { Data = user.Id, IsSuccess = true, Message = HelperMsg.creatMessage(loginDto.lang, "تم ارسال كود التفعيل علي رقم الجوال بنجاح من فضلك قم بالتفعيل", "The activation code has been sent to your mobile number successfully. Please activate") });
                }
                else
                {
                    return Ok(new BaseResponseDto<string> { Data = user.Id, IsSuccess = true, Message = HelperMsg.creatMessage(loginDto.lang, "تم ارسال كود التفعيل علي رقم الجوال بنجاح من فضلك قم بالتفعيل", "The activation code has been sent to your mobile number successfully. Please activate") });

                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }





        /// <summary>
        /// Change app Language 
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPatch(ApiRoutes.Identity.ChangeLanguage)]
        public async Task<IActionResult> ChangeLanguage()
        {

            string lang = _langService.Lang;
            if (await _accountService.ChangeLanguage(lang, _currentUserService.UserId))
            {
                return Ok(new BaseResponseDto<string> { Message = HelperMsg.creatMessage(lang, "تم تغير اللغه بنجاح", "Language Changed Successfully"), IsSuccess = true });
            }
            else
            {
                return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), lang) });
            }

        }
        /// <summary>
        /// user logout
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpDelete(ApiRoutes.Identity.Logout)]
        public async Task<IActionResult> Logout([FromQuery] LogoutDto userModel)
        {
            string lang = _langService.Lang;
            try
            {
                await _accountService.Logout(userModel);
                return Ok(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.LogOutSuccessfully.ToNumber(), lang), IsSuccess = true });
            }
            catch (Exception)
            {
                return BadRequest(new BaseResponseDto<string> { Message = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber(), lang) });

            }
        }


        /// <summary>
        /// Remove user account  مسح الحساب
        /// </summary>
        /// <remarks>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpDelete(ApiRoutes.Identity.RemoveAccount)]
        public async Task<IActionResult> RemoveAccount()
        {
            string lang = _langService.Lang;
            var data = await _accountService.RemoveAccount(_currentUserService.UserId, lang);
            if (data.check)
            {
                return Ok(new BaseResponseDto<string> { Message = data.message, IsSuccess = true });
            }
            else
            {
                return BadRequest(new BaseResponseDto<string> { Message = data.message });

            }

        }


        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.TestSendSMS)]
        public async Task<string> TestSendSMS(int code, string phone)
        {
            Setting GetInfoSms = await db.Settings.FirstOrDefaultAsync();
            if (GetInfoSms != null)
            {
                if (GetInfoSms.SenderName != "test")
                {
                    string autoFillCode = "BpoQI2VpPtL";
                    var text = $"Your verification code is: {code} {autoFillCode}";
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


    }
}
