using AAITHelper;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.DTO.ChatDTO;
using Asasy.Domain.Entities.Chat;
using Asasy.Domain.Enums;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Chat;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatServices;
        private readonly ApplicationDbContext _context;

        public ChatHub(IChatService chatServices, ApplicationDbContext context)
        {
            _chatServices = chatServices;
            _context = context;
        }

        public async Task SendMessage(string SenderId, string ReceiverId, string Text,int? adsId, int Type = 0, int Duration = 0)
        {
            if (!string.IsNullOrEmpty(SenderId) && !string.IsNullOrEmpty(ReceiverId) && !string.IsNullOrEmpty(Text))
            {
                _ = await _chatServices.AddNewMessage(SenderId, ReceiverId, Text,adsId, Type, Duration);

                List<string> ListContext = _context.ConnectUser.Where(x => x.UserId == ReceiverId).Select(x => x.ContextId).ToList();

                if (ListContext.Count > 0)
                {
                    NewMessageDto data = new NewMessageDto();
                    data.message = Text;
                    data.SenderId = SenderId;
                    data.ReceiverId = ReceiverId;
                    data.Date = DateTime.Now.ToString("hh:mm tt");
                    data.Type = Type;
                    data.Duration = Duration;


                    foreach (var ContextId in ListContext)
                    {
                        await Clients.Client(ContextId).SendAsync("receiveMessage", data);
                    }
                }
                else
                {
                    var parentDeviceIds = await _context.DeviceIds.Where(x => x.UserId == ReceiverId).Select(x => new AAITHelper.ModelHelper.DeviceIdModel
                    {
                        DeviceId = x.DeviceId_,
                        DeviceType = x.DeviceType,
                        FkUser = x.ProjectName,
                        ProjectName = x.ProjectName
                    }).ToListAsync();


                    int typeMessage = -2;
                    Dictionary<string, string> moreData = new Dictionary<string, string>();
                    moreData.Add("type", typeMessage.ToString());

                    var provider = _context.Users.Where(d => d.Id == ReceiverId).FirstOrDefault();

                    var textAr = "لديك رساله جديده";
                    var textEn = "You have a new message";

                    NotificationHelper
                        .SendPushNewNotificationAsync(parentDeviceIds, moreData, provider.Lang == "ar" ? textAr : textEn);

                }
            }
        }

        //To Connect in Mobile
        public async Task Connect(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectUser connect = new ConnectUser
                {
                    UserId = userId,
                    ContextId = Context.ConnectionId,
                    date= DateTime.Now
                };
                _context.ConnectUser.Add(connect);
                _context.SaveChanges();
                await Clients.All.SendAsync("connected", true);
            }
        }

        //To DisConnect in Mobile
        public async Task DisConnect(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var ListContext = _context.ConnectUser.Where(x => x.UserId == userId).ToList();
                if (ListContext.Count > 0)
                {
                    _context.ConnectUser.RemoveRange(ListContext);
                    _context.SaveChanges();
                }
                await Clients.All.SendAsync("disconnected", false);
            }
        }



        public async Task ConnectToOnline(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = _context.Users.Where(d => d.Id == userId).FirstOrDefault();
                if (user != null)
                {
                    user.IsOnline = true;
                    _context.SaveChanges();
                    await Clients.All.SendAsync("connected", true);
                }
            }
        }


        public async Task DisConnectToOfline(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = _context.Users.Where(d => d.Id == userId).FirstOrDefault();
                if (user != null)
                {
                    user.IsOnline = false;
                    user.LastAppearance = HelperDate.GetCurrentDate();
                    _context.SaveChanges();
                    await Clients.All.SendAsync("disconnected", false);
                }
            }
        }

    }
}
