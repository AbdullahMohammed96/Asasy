using AAITHelper.ModelHelper;
using AAITHelper;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.ViewModel.Complaints;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Auth;
using Asasy.Service.DashBoard.Contract.ComplaintsInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Asasy.Service.DashBoard.Implementation.ComplaintsImplementation
{
    public class ComplaintsService : IComplaintsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ComplaintsService(ApplicationDbContext context, ICurrentUserService currentUserService = null)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<List<ComplaintsListViewModel>> Complaints()
        {
            var Complaints = _context.Complaints.Select(c => new ComplaintsListViewModel
            {
                Id = c.Id,
                CreationDate = c.CreationDate.ToString("dd/MM/yyyy"),
                IsReplay = c.IsReplay,
                Replay = c.Replay != null ? c.Replay : "-----",
                Message = c.Message,
                Name = c.Name,
                Phone = c.Phone + " " + c.PhoneCode,
                CodeComplaint = c.CodeComplaint
            }).OrderByDescending(o => o.Id).ToList();

            return Complaints;

        }

        public async Task<bool> DeleteComplaints(int id)
        {
            var complaint = _context.Complaints.Where(d => d.Id == id).FirstOrDefault();

            complaint.IsDelete = true;
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> ReplayToComplaints(int id, string replay, string userName)
        {
            var complaint = _context.Complaints.Where(d => d.Id == id).FirstOrDefault();
            var user = _context.Users.Where(d => d.UserName == userName).FirstOrDefault();

            if (complaint != null)
            {
                complaint.Replay = replay;
                complaint.IsReplay = true;
                complaint.EmployeeId = user.Id;

                NotifyUser notify = new NotifyUser
                {
                    UserId = complaint.UserId,
                    AdsId = 0,
                    Date = HelperDate.GetCurrentDate(),
                    Show = false,
                    TextAr = " تم الرد علي الشكوي الخاصه بك من الادمن ",
                    TextEn = "Your complaint has been answered by the admin.",
                    Type = (int)NotifyTypes.ReplayToComplaints
                };

                _context.NotifyUsers.Add(notify);

                _context.SaveChanges();



                var parentDeviceIds = await _context.DeviceIds.Where(x => x.UserId == notify.UserId).Select(x => new DeviceIdModel
                {
                    DeviceId = x.DeviceId_,
                    DeviceType = x.DeviceType,
                    FkUser = x.ProjectName,
                    ProjectName = x.ProjectName
                })
              .ToListAsync();

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("info", complaint.Id.ToString());
                data.Add("type", notify.Type.ToString());

                var userFcm = _context.Users.Where(d => d.Id == complaint.UserId).FirstOrDefault();

                NotificationHelper
                    .SendPushNewNotificationAsync(parentDeviceIds, data, HelperMsg.creatMessage(userFcm.Lang, notify.TextAr, notify.TextEn));

                return true;
            }
            return false;

        }
    }
}
