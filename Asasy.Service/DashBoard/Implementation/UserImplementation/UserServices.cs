using Asasy.Domain.ViewModel.Users;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.UserInterfaces;
using Microsoft.EntityFrameworkCore;
using Asasy.Domain.Common.PathUrl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using AAITHelper;
using Asasy.Domain.Common.Helpers;
using AAITHelper.ModelHelper;

namespace Asasy.Service.DashBoard.Implementation.UserImplementation
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _context;
        private readonly Api.Contract.Auth.IUserServices _userServices;
        private readonly IHelper _helper;

        public UserServices(ApplicationDbContext context, Api.Contract.Auth.IUserServices userServices = null, IHelper helper = null)
        {
            _context = context;
            _userServices = userServices;
            _helper = helper;
        }


        public async Task<List<UsersViewModel>> GetUsers()
        {
            var Users = await _context.Users.Where(u => u.TypeUser==(int)UserType.Client)
                                            .Select(u => new UsersViewModel
                                            {
                                                Id = u.Id,
                                                UserName = u.user_Name,
                                                Email = u.Email,
                                                Phone = u.PhoneNumber + " " + u.PhoneCode,
                                                Image = DashBordUrl.baseUrlHost + u.ImgProfile,
                                                IsActive = u.IsActive,
                                                Date = u.PublishDate
                                            }).OrderByDescending(o=>o.Date).ToListAsync();
            return Users;
        }
        public async Task<bool> ChangeState(string UserId)
        {
            ApplicationDbUser user = await _context.Users.FindAsync(UserId);
            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();


            if (!user.IsActive)
            {
                NotifyUser notify = new NotifyUser
                {
                    Date = HelperDate.GetCurrentDate(),
                    Show = false,
                    TextAr = "تم حظرك من الاداره",
                    TextEn = "You have been banned from the administration",
                    Type = (int)NotifyTypes.BlockUser,
                    UserId = UserId,
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
                data.Add("info", UserId);
                data.Add("type", notify.Type.ToString());


                var userFcm = _context.Users.Where(d => d.Id == UserId).FirstOrDefault();

                NotificationHelper
                    .SendPushNewNotificationAsync(parentDeviceIds, data, HelperMsg.creatMessage(userFcm.Lang, notify.TextAr, notify.TextEn));



            }

            return user.IsActive;
        }

        public async Task<CheckDataViewModel> CreateUser(CreateUserViewModel model)
        {
            if (await _userServices.CheckValidatePhone(model.Phone))
            {
                return new CheckDataViewModel()
                {
                    Check = false,
                    Message = "هذا الرقم مسجل من قبل"
                };
            }

            if (model.Email != null && await _userServices.CheckValidateEmail(model.Email))
            {
                return new CheckDataViewModel()
                {
                    Check = false,
                    Message = "هذا الايميل مسجل من قبل"
                };
            }

            try
            {

                ApplicationDbUser provider = new ApplicationDbUser
                {
                    Email = model.Email != null ? model.Email : "",
                    UserName = model.Phone + HelperNumber.GetRandomNumber() + "@yahoo.com",
                    user_Name = model.Name,
                    ShowPassword = "",
                    ImgProfile = _helper.Upload(model.Image, (int)FileName.Users), //"images/Users/blackImg.png",
                    IsActive = true,
                    ActiveCode = true,
                    CloseNotify = false,
                    PublishDate = HelperDate.GetCurrentDate(),
                    Code = 1234,
                    PhoneNumber = model.Phone,
                    PhoneCode = model.Code,
                    TypeUser = (int)UserType.Client,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Wallet = 0,
                    Lang="ar",
                    
                };
                await _context.Users.AddAsync(provider);
                await _context.SaveChangesAsync();
                return new CheckDataViewModel()
                {
                    Check = true,
                    Message = "تم الاضافه بنجاح"
                };
            }
            catch (Exception)
            {
                return new CheckDataViewModel()
                {
                    Check = false,
                    Message = "حدث خطا ما"
                };
            }
        }

        public async Task<CheckDataViewModel> Delete(string id)
        {

            var foundedUser = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (foundedUser == null)
            {
                return new CheckDataViewModel()
                {
                    Check = false,
                    Message = "هذا المستخدم غير موجود"
                };
            }
            if (foundedUser.Wallet > 0)
            {
                return new CheckDataViewModel()
                {
                    Check = false,
                    Message = "لا يمكن حذف مستخدم لديه رصيد بمحفظته"
                };

            }
           

            foundedUser.user_Name = foundedUser.user_Name + Guid.NewGuid().ToString();
            foundedUser.PhoneNumber = foundedUser.PhoneNumber + Guid.NewGuid().ToString();
            foundedUser.Email = foundedUser.Email + Guid.NewGuid().ToString();
            foundedUser.NormalizedEmail = foundedUser.NormalizedEmail + Guid.NewGuid().ToString();
            foundedUser.UserName = foundedUser.UserName + Guid.NewGuid().ToString();
            foundedUser.NormalizedUserName = foundedUser.NormalizedUserName + Guid.NewGuid().ToString();
            foundedUser.IsDeleted = true;

            var adsForUser = _context.AdvertsmentDetails.Where(d => d.UserId == id).ToList();
            adsForUser.ForEach(a => a.IsDelete = true);

            await _context.SaveChangesAsync();


            var parentDeviceIds = await _context.DeviceIds.Where(x => x.UserId == id).Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.ProjectName,
                ProjectName = x.ProjectName
            })
            .ToListAsync();

            int type = -3;
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("info", id);
            data.Add("type", type.ToString());


            NotificationHelper
                .SendPushNewNotificationAsync(parentDeviceIds, data, foundedUser.Lang == "ar" ?  "تم حذف حسابك من الاداره": "Your account has been deleted from the administration.");



            var deviceIds = _context.DeviceIds.Where(d => d.UserId == id).ToList();
            _context.DeviceIds.RemoveRange(deviceIds);
            await _context.SaveChangesAsync();



            return new CheckDataViewModel()
            {
                Check = true,
                Message = "تم الحذف بنجاح"
            }; ;
        }
    }
}
