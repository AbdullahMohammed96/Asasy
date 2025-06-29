using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.ViewModel.Settings;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.SettingServices;
using Microsoft.EntityFrameworkCore;

namespace Asasy.Service.DashBoard.Implementation.SettingServices
{
    public class SettingServices : ISettingServices
    {
        private readonly ApplicationDbContext _context;

        public SettingServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<SettingEditViewModel> GetSetting(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var setting = await _context.Settings.FindAsync(id);

            SettingEditViewModel editsetting = new SettingEditViewModel
            {
                Id = setting.Id,
                CondtionsArClient = setting.CondtionsArClient,
                CondtionsEnClient = setting.CondtionsEnClient,
                //CondtionsArDelegt = setting.CondtionsArProvider,
                //CondtionsEnDelegt = setting.CondtionsEnProvider,
                //AboutUsArClient = setting.AboutUsArClient,
                //AboutUsEnClient = setting.AboutUsEnClient,
                //AboutUsArDelegt = setting.AboutUsArProvider,
                //AboutUsEnDelegt = setting.AboutUsEnProvider,
                ApplicationId = setting.ApplicationId,
                SenderName = setting.SenderName,
                PasswordSms = setting.PasswordSms,
                Phone = setting.Phone,
                SenderId = setting.SenderId,
                UserNameSms = setting.UserNameSms,
                Email = setting.Email,
                Lat = setting.Lat,
                Lng = setting.Lng,
                Location = setting.Address,
                CommissionApp = setting.CommissionApp * 100,
                ContactUsAr=setting.ContactUsAr,
                ContactUsEn=setting.ContactUsEn,
                DiscountSystemAr=setting.DiscountSystemAr,
                DiscountSystemEn = setting.DiscountSystemEn,
                PolicyAr = setting.PolicyAr,
                PolicyEn=setting.PolicyEn,
                FooterAr =setting.FooterAr,
                FooterEn = setting.FooterEn
            };
            if (setting == null)
            {
                return null;
            }

            return editsetting;
        }

        public async Task<bool> EditSetting(SettingEditViewModel editSettingViewModel)
        {
     
        Setting setting = await _context.Settings.FindAsync(editSettingViewModel.Id);
            setting.Id = editSettingViewModel.Id;
            setting.CondtionsArClient = editSettingViewModel.CondtionsArClient;
            setting.CondtionsEnClient = editSettingViewModel.CondtionsEnClient;
            //setting.CondtionsArProvider = editSettingViewModel.CondtionsArDelegt;
            //setting.CondtionsEnProvider = editSettingViewModel.CondtionsEnDelegt;
            //setting.AboutUsArClient = editSettingViewModel.AboutUsArClient;
            //setting.AboutUsEnClient = editSettingViewModel.AboutUsEnClient;
            //setting.AboutUsArProvider = editSettingViewModel.AboutUsArDelegt;
            //setting.AboutUsEnProvider = editSettingViewModel.AboutUsEnDelegt;
            setting.ApplicationId = editSettingViewModel.ApplicationId;
            setting.SenderName = editSettingViewModel.SenderName;
            setting.PasswordSms = editSettingViewModel.PasswordSms;
            setting.Phone = editSettingViewModel.Phone;
            setting.SenderId = editSettingViewModel.SenderId;
            setting.UserNameSms = editSettingViewModel.UserNameSms;
            setting.Email = editSettingViewModel.Email;
            setting.Lat = editSettingViewModel.Lat;
            setting.Lng = editSettingViewModel.Lng;
            setting.Address = editSettingViewModel.Location;
            setting.CommissionApp = editSettingViewModel.CommissionApp / 100;
            setting.PolicyAr = editSettingViewModel.PolicyAr;
            setting.PolicyEn = editSettingViewModel.PolicyEn;
            setting.DiscountSystemAr = editSettingViewModel.DiscountSystemAr;
            setting.DiscountSystemEn = editSettingViewModel.DiscountSystemEn;
            setting.ContactUsAr = editSettingViewModel.ContactUsAr;
            setting.ContactUsEn = editSettingViewModel.ContactUsEn;
            setting.FooterAr = editSettingViewModel.FooterAr;
            setting.FooterEn = editSettingViewModel.FooterEn;
            return await _context.SaveChangesAsync() > 0;
        }

        public bool SettingExists(int id)
        {
            return _context.Settings.Any(e => e.Id == id);
        }
    }
}
