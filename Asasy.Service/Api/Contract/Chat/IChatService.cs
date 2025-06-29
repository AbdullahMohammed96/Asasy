using Asasy.Domain.DTO.ChatDTO;
using Asasy.Domain.Entities.Chat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Contract.Chat
{
    public interface IChatService
    {
        Task<bool> AddNewMessage(string SenderId, string ReceiverId, string Text, int? AdsId, int Type = 0, int Duration = 0);
        Task<List<string>> GetDevicesId(string UserId);
        Task<string> GetReceiverName(string UserId);
        Task<string> GetReceiverImg(string UserId);
        Task<List<ListUsersMyChatDto>> GetListUsersMyChat(string UserId, string lang);
        Task<List<ListMessageTwoUsersDto>> GetListMessageTwoUsersDto(string UserId, int OrderId, int pageNumber);
        Task<AdInfoDto> ChatAdInfo(int adId,string lang);
    }
}
