using Asasy.Domain.ViewModel.Notification;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.NotificationInterfaces
{
    public interface INotificationServices
    {
        Task<List<HistoryNotificationViewModel>> GetHistoryNotify();
        Task<List<UsersViewModel>> GetUsers();
        Task<List<UsersViewModel>> GetDeleget();
        Task<bool> Send(string msg, string employees, string providers);
        Task<bool> Notify(string textAr, string textEn, string fkProvider, int stutes, int orderId = 0);
    }
}
