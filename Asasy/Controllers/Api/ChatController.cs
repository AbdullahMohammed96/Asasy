using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.DTO.ChatDTO;
using Asasy.Domain.Entities.Chat;
using Asasy.Domain.Enums;
using Asasy.Service.Api.Contract.Chat;
using Asasy.Service.Api.Contract.Lang;
using Asasy.Service.Api.Implementation.FilterValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asasy.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "ChatAPI")]
    [PermessionFilterAttribute]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IChatService _chatServices;
        private readonly IHelper _helper;
        private readonly ILangService _langService;

        public ChatController(IChatService chatServices, IHelper helper, ILangService langService)
        {
            _chatServices = chatServices;
            _helper = helper;
            _langService = langService;
        }

        //دى هتجيب كل الشاتات الخاصه بشخص معين 
        [HttpPost(ApiRoutes.Chat.ListUsersMyChat)]
        public async Task<IActionResult> ListUsersMyChat()
        {
            string lang = _langService.Lang;
            try
            {
                string UserId = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;

                List<ListUsersMyChatDto> ListUsers = await _chatServices.GetListUsersMyChat(UserId, lang);

                return Json(new { key = 1, data = ListUsers });
            }
            catch (Exception ex)
            {
                return Json(new { key = 0, data = ex.Message });
            }
        }

        // دى هتجيب كل الرسائل الى فى شات معين هنا كان الشات على الطلب لو اللوجيك على حاجه تانيه خلاف الطلب نظبطها للوجيك الى انا عاوزه
        [HttpPost(ApiRoutes.Chat.ListMessagesUser)]
        public async Task<ActionResult> ListMessagesUser(int ChatId, int pageNumber = 1)
        {
            string userId = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
            List<ListMessageTwoUsersDto> ListMessages = await _chatServices.GetListMessageTwoUsersDto(userId, ChatId, pageNumber);

            return Json(new { key = 1, data = ListMessages });
        }

        // دى علشان الموبايل يقدر يرفع اى ملف فى الشات الفكره هنا انه بيرفع الملف عندى سواء كان صورة فيديو ...هرجعله لينك هيحطه عندى فى ال الرسائل عندى
        //[HttpPost(ApiRoutes.Chat.UploadNewFile)]
        //public ActionResult UploadFile([FromForm] UploadFileDto file)
        //{
        //    string fileName = _helper.Upload(file.File, (int)FileName.ChatFiles);

        //    return Json(new { key = 1, data = fileName, msg = "تم الارسال" });
        //}
        [HttpPost(ApiRoutes.Chat.ListAllMessages)]
        public async Task<ActionResult> ListAllMessages(int ChatId, int pageNumber = 1)
        {
            string userId = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
            string lang = _langService.Lang;

            List<ListMessageTwoUsersDto> ListMessages = await _chatServices.GetListMessageTwoUsersDto(userId, ChatId, pageNumber);
            var AdObject = await _chatServices.ChatAdInfo(ChatId,lang);
            var data = new GetAllMessagesDto
            {
                ad = AdObject,
                ListMessages = ListMessages
            };
            return Json(new { key = 1, data = data });
        }

    }
}
