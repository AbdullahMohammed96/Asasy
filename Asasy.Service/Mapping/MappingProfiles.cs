using AutoMapper;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.DTO.AppClientDTO.AboutUsClientDto;
using Asasy.Domain.DTO.AppClientDTO.CondtionsClientDto;
using Asasy.Domain.DTO.AppClientDTO.ContactUsClientDto;
using Asasy.Domain.DTO.AppClientDTO.QuestionsClientDto;
using Asasy.Domain.DTO.AppProviderDTO.AboutUsProviderDto;
using Asasy.Domain.DTO.AppProviderDTO.CondtionsProviderDto;
using Asasy.Domain.DTO.AppProviderDTO.ContactUsProviderDto;
using Asasy.Domain.DTO.AppProviderDTO.QuestionsProviderDto;
using Asasy.Domain.DTO.AuthDTO;
using Asasy.Domain.DTO.OrderDTO;
using Asasy.Domain.DTO.SettingDTO;
using Asasy.Domain.DTO.SocialMediaDTO;
using Asasy.Domain.Entities;
using Asasy.Domain.Entities.AdditionalTables;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Entities.UserTables;

namespace Asasy.Service.Mapping
{
    internal class MappingProfiles : Profile
    {


        #region App
        public static MapperConfiguration SettingsProfile(string lang)
        {
            var configuration = new MapperConfiguration(
            cfg =>
            cfg.CreateMap<Setting, SettingDto>()
                .ForMember(dto => dto.phone, conf => conf.MapFrom(ol => ol.Phone))
                .ForMember(dto => dto.aboutUs, conf => conf.MapFrom(ol => ol.ChangeLangAboutUsClient(lang)))
                .ForMember(dto => dto.aboutUsProvider, conf => conf.MapFrom(ol => ol.ChangeLangAboutUsProvider(lang)))
                .ForMember(dto => dto.condtions, conf => conf.MapFrom(ol => ol.ChangeLangCondtionClient(lang)))
                .ForMember(dto => dto.condtionsProvider, conf => conf.MapFrom(ol => ol.ChangeLangCondtionProvider(lang)))
            );
            return configuration;
        }
        public static MapperConfiguration AboutUsClientProfile(string lang)
        {
            var configuration = new MapperConfiguration(
            cfg =>
            cfg.CreateMap<Setting, AboutUsClientDto>()
                .ForMember(dto => dto.aboutUs, conf => conf.MapFrom(ol => ol.ChangeLangAboutUsClient(lang)))
            );
            return configuration;
        }
        public static MapperConfiguration ContactUsClientProfile()
        {
            var configuration = new MapperConfiguration(
            cfg =>
            cfg.CreateMap<ContactUs, ContactUsClientListDto>()
                .ForMember(dto => dto.email, conf => conf.MapFrom(ol => ol.Email))
                .ForMember(dto => dto.id, conf => conf.MapFrom(ol => ol.Id))
                .ForMember(dto => dto.msg, conf => conf.MapFrom(ol => ol.Msg))
                .ForMember(dto => dto.userName, conf => conf.MapFrom(ol => ol.UserName))
            );
            return configuration;
        }
        public static MapperConfiguration CondtionClientProfile(string lang)
        {
            var configuration = new MapperConfiguration(
            cfg =>
            cfg.CreateMap<Setting, CondtionsClientDto>()
             .ForMember(dto => dto.condtions, conf => conf.MapFrom(ol => ol.ChangeLangCondtionClient(lang)))
            );
            return configuration;
        }


        public static MapperConfiguration AboutUsProviderProfile(string lang)
        {
            var configuration = new MapperConfiguration(
            cfg =>
            cfg.CreateMap<Setting, AboutUsProviderDto>()
                .ForMember(dto => dto.aboutUs, conf => conf.MapFrom(ol => ol.ChangeLangAboutUsProvider(lang)))
            );
            return configuration;
        }
        public static MapperConfiguration ContactUsProviderProfile()
        {
            var configuration = new MapperConfiguration(
            cfg =>
            cfg.CreateMap<ContactUs, ContactUsProviderListDto>()
                .ForMember(dto => dto.email, conf => conf.MapFrom(ol => ol.Email))
                .ForMember(dto => dto.id, conf => conf.MapFrom(ol => ol.Id))
                .ForMember(dto => dto.msg, conf => conf.MapFrom(ol => ol.Msg))
                .ForMember(dto => dto.userName, conf => conf.MapFrom(ol => ol.UserName))
            );
            return configuration;
        }
        public static MapperConfiguration CondtionProviderProfile(string lang)
        {
            var configuration = new MapperConfiguration(
            cfg =>
            cfg.CreateMap<Setting, CondtionsProviderDto>()
             .ForMember(dto => dto.condtions, conf => conf.MapFrom(ol => ol.ChangeLangCondtionProvider(lang)))
            );
            return configuration;
        }
        #endregion

        #region Logic
        public static MapperConfiguration OrdersProviderMapping(string lang)
        {
            var configuration = new MapperConfiguration(
                              cfg =>
                              cfg.CreateMap<Order, OrderListDto>()
                                  .ForMember(dto => dto.date, conf => conf.MapFrom(ol => ol.DateTime.ToString("dd/MM/yyyy HH:mm")))
                                  .ForMember(dto => dto.location, conf => conf.MapFrom(ol => ol.User.Location))
                                  .ForMember(dto => dto.name, conf => conf.MapFrom(ol => ol.User.user_Name))
                                  .ForMember(dto => dto.orderId, conf => conf.MapFrom(ol => ol.Id))
                                  .ForMember(dto => dto.stutes, conf => conf.MapFrom(ol => Helper.StutesText(ol.Stutes, lang))));
            return configuration;

        }
        public static MapperConfiguration OrdersUserMapping(string lang)
        {
            var configuration = new MapperConfiguration(
                              cfg =>
                              cfg.CreateMap<Order, OrderListDto>()
                                  .ForMember(dto => dto.date, conf => conf.MapFrom(ol => ol.DateTime.ToString("dd/MM/yyyy HH:mm")))
                                  .ForMember(dto => dto.location, conf => conf.MapFrom(ol => ol.User.Location))
                                  .ForMember(dto => dto.name, conf => conf.MapFrom(ol => ol.Provider.user_Name))
                                  .ForMember(dto => dto.orderId, conf => conf.MapFrom(ol => ol.Id))
                                  .ForMember(dto => dto.stutes, conf => conf.MapFrom(ol => Helper.StutesText(ol.Stutes, lang))));
            return configuration;

        }
        #endregion


        #region User

        public static MapperConfiguration UserInfo(string token)
        {
            var configuration = new MapperConfiguration(
            cfg =>
            cfg.CreateMap<ApplicationDbUser, UserInfoDTO>()
                    .ForMember(dto => dto.id, conf => conf.MapFrom(ol => ol.Id))
                    .ForMember(dto => dto.imgProfile, conf => conf.MapFrom(ol => DashBordUrl.baseUrlHost + ol.ImgProfile))
                    .ForMember(dto => dto.userName, conf => conf.MapFrom(ol => ol.user_Name))
                    .ForMember(dto => dto.phone, conf => conf.MapFrom(ol => ol.PhoneNumber))
                    .ForMember(dto => dto.typeUser, conf => conf.MapFrom(ol => ol.TypeUser))
                    .ForMember(dto => dto.email, conf => conf.MapFrom(ol => ol.Email))
                    .ForMember(dto => dto.lang, conf => conf.MapFrom(ol => ol.Lang))
                    .ForMember(dto => dto.code, conf => conf.MapFrom(ol => ol.Code))
                    .ForMember(dto => dto.token, conf => conf.MapFrom(ol => token))
                    .ForMember(dto => dto.ActiveCode, conf => conf.MapFrom(ol => ol.ActiveCode))
                    .ForMember(dto => dto.closeNotify, conf => conf.MapFrom(ol => ol.CloseNotify))
                    .ForMember(dto => dto.location, conf => conf.MapFrom(ol => ol.Location))



            );
            return configuration;
        }

        public static MapperConfiguration ProviderInfo(string token)
        {
            var configuration = new MapperConfiguration(
            cfg =>
            cfg.CreateMap<ApplicationDbUser, UserInfoDTO>()
                    .ForMember(dto => dto.id, conf => conf.MapFrom(ol => ol.Id))
                    .ForMember(dto => dto.imgProfile, conf => conf.MapFrom(ol => DashBordUrl.baseUrlHost + ol.ImgProfile))
                    .ForMember(dto => dto.userName, conf => conf.MapFrom(ol => ol.user_Name))
                    .ForMember(dto => dto.phone, conf => conf.MapFrom(ol => ol.PhoneNumber))
                    .ForMember(dto => dto.typeUser, conf => conf.MapFrom(ol => ol.TypeUser))
                    .ForMember(dto => dto.email, conf => conf.MapFrom(ol => ol.Email))
                    .ForMember(dto => dto.lang, conf => conf.MapFrom(ol => ol.Lang))
                    .ForMember(dto => dto.code, conf => conf.MapFrom(ol => ol.Code))
                    .ForMember(dto => dto.token, conf => conf.MapFrom(ol => token))
                    .ForMember(dto => dto.ActiveCode, conf => conf.MapFrom(ol => ol.ActiveCode))
                    .ForMember(dto => dto.closeNotify, conf => conf.MapFrom(ol => ol.CloseNotify))


            );
            return configuration;
        }
        #endregion

    }
}
