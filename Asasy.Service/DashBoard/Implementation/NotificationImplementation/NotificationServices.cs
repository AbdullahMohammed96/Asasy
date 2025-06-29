using AAITHelper.ModelHelper;
using AAITHelper;
using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Notification;
using Asasy.Domain.ViewModel.Settings;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.NotificationInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAITHelper.Enums;
using Asasy.Domain.Entities.UserTables;
using Microsoft.AspNetCore.Mvc;
using Asasy.Domain.Common.Helpers;

namespace Asasy.Service.DashBoard.Implementation.NotificationImplementation
{
    public class NotificationServices : INotificationServices
    {
        private readonly ApplicationDbContext _context;

        public NotificationServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HistoryNotificationViewModel>> GetHistoryNotify()
        {
            var UserNotifies = await _context.HistoryNotify
                                             .Select(x => new HistoryNotificationViewModel
                                             {
                                                 id = x.Id,
                                                 Text = x.Text,
                                                 TextDate = x.Date,
                                                 UserNotifyCount = x.UserCountNotify,
                                                 ProviderNotifyCount = x.ProviderCountNotify,
                                             }).OrderByDescending(a=>a.id).ToListAsync();
            return UserNotifies;
        }

        public async Task<List<UsersViewModel>> GetUsers()
        {
            var users = await _context.Users.Where(u => u.IsActive && u.TypeUser == (int)UserType.Client && !u.IsDeleted)
                                            .Select(x => new UsersViewModel
                                            {
                                                id = x.Id,
                                                name = x.user_Name
                                            }).ToListAsync();
            return users;
        }
        public async Task<List<UsersViewModel>> GetDeleget()
        {
            var Deleget = await _context.Users.Where(u => u.IsActive && u.TypeUser == (int)UserType.deleget && !u.IsDeleted)
                                            .Select(x => new UsersViewModel
                                            {
                                                id = x.Id,
                                                name = x.user_Name
                                            }).ToListAsync();
            return Deleget;
        }
        public async Task<bool> Send(string msg, string employees, string providers)
        {

            List<NotifyUser> NotifyUserers = new List<NotifyUser>();
            List<NotifyDelegt> NotifyProviders = new List<NotifyDelegt>();

            if (employees != null)
            {
                var employeeArr = employees.Split(',');
                foreach (var clientId in employeeArr)
                {
                    NotifyUserers.Add(new NotifyUser()
                    {
                        Date = HelperDate.GetCurrentDate(),
                        UserId = clientId,
                        Show = false,
                        TextAr = msg,
                        TextEn = msg,
                        Type = NotifyTypes.NotiyFromDashBord.ToNumber(),
                        AdsId = 0

                    });

                    //await Notify(msg, msg, clientId, NotifyTypes.NotiyFromDashBord.ToNumber());


                    var parentDeviceIds = await _context.DeviceIds.Where(x => x.UserId == clientId).Select(x => new DeviceIdModel
                    {
                        DeviceId = x.DeviceId_,
                        DeviceType = x.DeviceType,
                        FkUser = x.ProjectName,
                        ProjectName = x.ProjectName
                    })
             .ToListAsync();


                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data.Add("type", NotifyTypes.NotiyFromDashBord.ToNumber().ToString());

                    var user = _context.Users.Where(d => d.Id == clientId).FirstOrDefault();
                    if (user != null)
                    {
                        NotificationHelper
                      .SendPushNewNotificationAsync(parentDeviceIds, data, HelperMsg.creatMessage(user.Lang, msg, msg));


                    }


                }

                await _context.NotifyUsers.AddRangeAsync(NotifyUserers);
            }

            if (providers != null)
            {
                var ProvidersArr = providers.Split(',');
                foreach (var providerId in ProvidersArr)
                {
                    NotifyProviders.Add(new NotifyDelegt()
                    {
                        Date = HelperDate.GetCurrentDate(),
                        UserId = providerId,
                        Show = false,
                        TextAr = msg,
                        TextEn = msg,
                        Type = NotifyTypes.NotiyFromDashBord.ToNumber(),
                        OrderId = 0
                    });

                    await Notify(msg, msg, providerId, NotifyTypes.NotiyFromDashBord.ToNumber());

                }

                await _context.NotifyDelegts.AddRangeAsync(NotifyProviders);
            }

            await _context.HistoryNotify.AddAsync(new()
            {
                Text = msg,
                Date = HelperDate.GetCurrentDate(),
                UserCountNotify = NotifyUserers.Count,
                ProviderCountNotify = NotifyProviders.Count,
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Notify(string textAr, string textEn, string fkProvider, int stutes, int orderId = 0)
        {

            var TypeUser = await _context.Users.Where(x => x.Id == fkProvider).Select(x => x.TypeUser).FirstOrDefaultAsync();

            NotifyViewModel Fcm = await _context.Settings
                    .Select(x => new NotifyViewModel
                    {
                        AppId = x.ApplicationId,
                        SenderId = x.SenderId
                    }).FirstOrDefaultAsync();

            List<DeviceIdModel> AllDeviceids = await _context.DeviceIds.Where(x => x.UserId == fkProvider).Select(x => new DeviceIdModel()
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName

            }).AsNoTracking().ToListAsync();
            HelperFcm.SendPushNotification(Fcm.AppId, Fcm.SenderId, AllDeviceids, null, textAr, stutes, TypeUser, orderId);
            return true;
        }

    }
}
