using AAITHelper;
using AAITHelper.Enums;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.DTO.ChatDTO;
using Asasy.Domain.Entities.Chat;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Chat;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Asasy.Domain.Enums.ChatMessage;

namespace Asasy.Service.Api.Implementation.Chat
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNewMessage(string SenderId, string ReceiverId, string Text, int? AdsId, int Type = 0, int Duration = 0)
        {
            try
            {
                var chat = _context.Chats.Where(d => (d.UserId == SenderId && d.ProviderId == ReceiverId) || (d.UserId == ReceiverId && d.ProviderId == SenderId)).FirstOrDefault();

                if (chat == null)
                {
                    Chats newChat = new Chats
                    {
                        UserId = SenderId,
                        ProviderId = ReceiverId,
                        AdsId = AdsId,

                    };

                    _context.Chats.Add(newChat);
                    try
                    {

                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        LogExption logExption = new LogExption
                        {
                            Date = DateTime.Now,
                            Exption = ex.Message,
                            ServiceName = "AddNewMessage",
                            UserId = ReceiverId + " , " + SenderId,
                        };

                        _context.LogExption.Add(logExption);
                        _context.SaveChanges();
                        throw;
                    }

                    Messages message = new Messages();
                    message.SenderId = SenderId;
                    message.ReceiverId = ReceiverId;
                    message.Text = Text;
                    message.TypeMessage = Type;
                    message.Duration = Duration;

                    message.DateSend = DateTime.UtcNow.AddHours(3);
                    message.IsDeletedReceiver = false;
                    message.IsDeletedSender = false;
                    message.LastMessage = LastMessage.LastMessage.ToNumber();
                    message.ChatId = newChat.Id;

                    await _context.Messages.AddAsync(message);
                    await _context.SaveChangesAsync();



                    return true;

                }

                Messages newmessage = new Messages();
                newmessage.SenderId = SenderId;
                newmessage.ReceiverId = ReceiverId;
                //newmessage.ProviderId = ProviderId;
                newmessage.Text = Text;
                newmessage.TypeMessage = Type;
                newmessage.Duration = Duration;

                newmessage.DateSend = DateTime.UtcNow.AddHours(3);
                newmessage.IsDeletedReceiver = false;
                newmessage.IsDeletedSender = false;
                newmessage.LastMessage = LastMessage.LastMessage.ToNumber();
                newmessage.ChatId = chat.Id;

                await _context.Messages.AddAsync(newmessage);
                int result = await _context.SaveChangesAsync();



                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<string>> GetDevicesId(string UserId)
        {
            List<string> Devices = await _context.DeviceIds.Where(x => x.UserId == UserId && x.DeviceId_ != null).Select(x => x.DeviceId_).AsNoTracking().ToListAsync();
            return Devices;
        }

        public async Task<List<ListMessageTwoUsersDto>> GetListMessageTwoUsersDto(string UserId, int ChatId, int pageNumber)
        {
            const int maxRows = 100;

            List<ListMessageTwoUsersDto> ListMessages = await _context.Messages.Where(x => x.ChatId == ChatId)
                .OrderByDescending(x => x.Id).Skip((pageNumber - 1) * maxRows).Take(maxRows)
               .Select(x => new ListMessageTwoUsersDto
               {
                   Id = x.Id,
                   Type = x.TypeMessage,
                   SenderId = x.SenderId,
                   SenderName = x.Sender.user_Name,
                   SenderImage = DashBordUrl.baseUrlHost + x.Sender.ImgProfile,
                   ReceiverId = x.ReceiverId,
                   ReceiverName = x.Receiver.user_Name,
                   ReceiverImage = DashBordUrl.baseUrlHost + x.Receiver.ImgProfile,
                   Message = x.Text,
                   Date = x.DateSend.ToString("hh:mm tt"),
                   ChatId = x.ChatId,
                   Duration = x.Duration
               }).OrderBy(x => x.Id).AsNoTracking().ToListAsync();

            return ListMessages;
        }

        public async Task<List<ListUsersMyChatDto>> GetListUsersMyChat(string UserId, string lang)
        {
            List<ListUsersMyChatDto> ListUsers = await (from chat in _context.Chats
                                                        where chat.Messages.Any(m => m.SenderId == UserId || m.ReceiverId == UserId)
                                                        let message = chat.Messages.OrderByDescending(m => m.Id).FirstOrDefault()
                                                        select new ListUsersMyChatDto
                                                        {
                                                            Id = message.Id,
                                                            ChatId = message.ChatId,
                                                            lastMsg = message.Text,
                                                            UserId = message.SenderId == UserId ? message.ReceiverId : message.SenderId,
                                                            UserImg = message.SenderId == UserId ? DashBordUrl.baseUrlHost + message.Receiver.ImgProfile : DashBordUrl.baseUrlHost + message.Sender.ImgProfile,
                                                            UserName = message.SenderId == UserId ? message.Receiver.user_Name : message.Sender.user_Name,
                                                            Date = message.DateSend.ToString("dd/MM/yyyy"),
                                                            adInfo = message.Chat.AdsId != null ? new AdInfoDto
                                                            {
                                                                adDate = message.Chat.Ad.CreationDate.ToString("dddd, dd MMMM yyyy"),
                                                                adId = message.Chat.AdsId.Value,
                                                                image = message.Chat.Ad.AdvertsmentImages.Select(m => DashBordUrl.baseUrlHost + m.Image).FirstOrDefault(),
                                                                price = message.Chat.Ad.Price,
                                                                title = message.Chat.Ad.Title
                                                            } : null,
                                                            //AdDate = message.Chat.AdsId != null ? message.Chat.Ad.CreationDate.ToString("dddd, dd MMMM yyyy") : null,
                                                            //AdId = message.Chat.AdsId != null ? message.Chat.AdsId.Value : 0,
                                                        }).OrderByDescending(o => o.ChatId).AsNoTracking().ToListAsync();

            return ListUsers;
        }

        public async Task<string> GetReceiverName(string UserId)
        {
            string ReceiverName = await _context.Users.Where(x => x.Id == UserId).Select(x => x.user_Name).FirstOrDefaultAsync();
            return ReceiverName;
        }
        public async Task<string> GetReceiverImg(string UserId)
        {
            string Img = await _context.Users.Where(x => x.Id == UserId).Select(x => x.ImgProfile).FirstOrDefaultAsync();
            return Img;
        }
        public async Task<AdInfoDto> ChatAdInfo(int ChatId, string lang)
        {

            var adsId = await _context.Chats.FirstOrDefaultAsync(x => x.Id == ChatId);
            if (adsId != null)
            {
                var Ad = await _context.AdvertsmentDetails
                                            .Where(x => x.Id == adsId.AdsId)
                                            .Select(x => new AdInfoDto
                                            {
                                                adDate = HelperDate.FormatDate(x.CreationDate, lang),
                                                creationDate = x.CreationDate.ToString("dddd, dd MMMM yyyy"),
                                                adId = x.Id,
                                                image = x.AdvertsmentImages.Select(m => DashBordUrl.baseUrlHost + m.Image).FirstOrDefault(),
                                                price = x.Price,
                                                title = x.Title,
                                            })
                                            .FirstOrDefaultAsync();
                return Ad;
            }
            return null;
        }
    }
}

