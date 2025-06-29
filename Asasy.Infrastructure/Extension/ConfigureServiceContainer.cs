using System.Globalization;
using System.Text;
using AAITPayment.MyFatoorah;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.DTO.AppClientDTO.ContactUsClientDto;
using Asasy.Domain.DTO.AuthApiDTO;
using Asasy.Domain.DTO.AuthDTO;
using Asasy.Domain.Entities.UserTables;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Auth;
using Asasy.Service.Api.Contract.Chat;
using Asasy.Service.Api.Contract.ClientLogic;
using Asasy.Service.Api.Contract.Copon;
using Asasy.Service.Api.Contract.Lang;
using Asasy.Service.Api.Contract.Logic;
using Asasy.Service.Api.Contract.Shared;
using Asasy.Service.Api.Implementation.Auth;
using Asasy.Service.Api.Implementation.Chat;
using Asasy.Service.Api.Implementation.ClientLogic;
using Asasy.Service.Api.Implementation.Copon;
using Asasy.Service.Api.Implementation.Lang;
using Asasy.Service.Api.Implementation.Logic;
using Asasy.Service.Api.Implementation.Shared;
using Asasy.Service.Contract.More;
using Asasy.Service.DashBoard.Contract.AdminInterfaces;
using Asasy.Service.DashBoard.Contract.AdvertsmentInterface;
using Asasy.Service.DashBoard.Contract.CitiesInterfaces;
using Asasy.Service.DashBoard.Contract.ComplaintsInterfaces;
using Asasy.Service.DashBoard.Contract.ContactUsInterfaces;
using Asasy.Service.DashBoard.Contract.CoponsInterfaces;
using Asasy.Service.DashBoard.Contract.DistrictInterface;
using Asasy.Service.DashBoard.Contract.HomeInterfaces;
using Asasy.Service.DashBoard.Contract.NotificationInterfaces;
using Asasy.Service.DashBoard.Contract.PackagesInterface;
using Asasy.Service.DashBoard.Contract.PaymentsInterfaces;
using Asasy.Service.DashBoard.Contract.ProhibitedGoodsInterfaces;
using Asasy.Service.DashBoard.Contract.QuestionsInterfaces;
using Asasy.Service.DashBoard.Contract.RatesInterfaces;
using Asasy.Service.DashBoard.Contract.RegionsInterfaces;
using Asasy.Service.DashBoard.Contract.ReportInterfaces;
using Asasy.Service.DashBoard.Contract.SettingServices;
using Asasy.Service.DashBoard.Contract.SliderInterfaces;
using Asasy.Service.DashBoard.Contract.SocialMediaInterfases;
using Asasy.Service.DashBoard.Implementation.AdminImplementation;
using Asasy.Service.DashBoard.Implementation.AdvertsmentImplementation;
using Asasy.Service.DashBoard.Implementation.CitiesImplementation;
using Asasy.Service.DashBoard.Implementation.ComplaintsImplementation;
using Asasy.Service.DashBoard.Implementation.ContactUsImplementation;
using Asasy.Service.DashBoard.Implementation.CoponsImplementation;
using Asasy.Service.DashBoard.Implementation.DistrictImplementation;
using Asasy.Service.DashBoard.Implementation.HomeImplementation;
using Asasy.Service.DashBoard.Implementation.NotificationImplementation;
using Asasy.Service.DashBoard.Implementation.PackageImplementation;
using Asasy.Service.DashBoard.Implementation.PaymentsImplementation;
using Asasy.Service.DashBoard.Implementation.ProhibitedGoodsImplementation;
using Asasy.Service.DashBoard.Implementation.QuestionImplementation;
using Asasy.Service.DashBoard.Implementation.RatesImplementation;
using Asasy.Service.DashBoard.Implementation.RegionsImplementation;
using Asasy.Service.DashBoard.Implementation.ReportImplementation;
using Asasy.Service.DashBoard.Implementation.SettingServices;
using Asasy.Service.DashBoard.Implementation.SliderImplementaion;
using Asasy.Service.DashBoard.Implementation.SocialMediaImplementation;
using Asasy.Service.Implementation.More;
using FirebaseAdmin;
using FluentValidation;
using FluentValidation.Resources;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using DashBoardImplementation = Asasy.Service.DashBoard.Implementation.UserImplementation;
using DashBoardInterface = Asasy.Service.DashBoard.Contract.UserInterfaces;

namespace Asasy.Infrastructure.Extension
{
    public static class ConfigureServiceContainer
    {
        public static void AddDbContextServices(this IServiceCollection services,
             IConfiguration Configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));


        }

        public static void AddSingletonServices(this IServiceCollection services)
        {


        }

        public static void AddLocalizationServices(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("ar"),
                    new CultureInfo("en")
                };

                options.DefaultRequestCulture = new RequestCulture(supportedCultures[1]);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

        }


        public static void AddFireBase(this IServiceCollection services)
        {
            // Initialize Firebase
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "athathy-db751-firebase-adminsdk-f0tz5-7a5df9cfdd.json")),

            });


        }

        public static void TimeOutServices(this IServiceCollection services, IWebHostEnvironment Environment)
        {
            services.AddDataProtection()
                              .SetApplicationName($"my-app-{Environment.EnvironmentName}")
                              .PersistKeysToFileSystem(new DirectoryInfo($@"{Environment.ContentRootPath}\keys"));

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(300);
            });


        }

        public static void AddDefaultIdentityServices(this IServiceCollection services)
        {

            services.AddDefaultIdentity<ApplicationDbUser>(options =>
            {
                // Default Password settings.
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            }).AddRoles<IdentityRole>().AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>();
        }
        public static void AddJwtServices(this IServiceCollection services, IConfiguration Configuration)
        {

            services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {

                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["Jwt:Site"],
                        ValidIssuer = Configuration["Jwt:Site"],

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"]))
                    };
                });

        }
        public static void AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped<IHelper, Helper>();
            services.AddScoped<IChatService, ChatService>();
        }

        public static void AddTransientServices(this IServiceCollection services)
        {
            #region FluentValidation
            services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
            services.AddTransient<IValidator<UserAddDTO>, RegisterDtoValidator>();

            services.AddValidatorsFromAssemblyContaining<ConfirmCodeDtoValidator>();
            services.AddTransient<IValidator<ConfirmCodeAddDto>, ConfirmCodeDtoValidator>();

            services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
            services.AddTransient<IValidator<LoginDto>, LoginDtoValidator>();

            services.AddValidatorsFromAssemblyContaining<ChangePasswordDtoValidator>();
            services.AddTransient<IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();

            services.AddValidatorsFromAssemblyContaining<ForgetPasswordDtoValidator>();
            services.AddTransient<IValidator<ForgetPasswordAddDto>, ForgetPasswordDtoValidator>();

            services.AddValidatorsFromAssemblyContaining<ChangePasswordByCodeDtoValidator>();
            services.AddTransient<IValidator<ChangePasswordByCodeDto>, ChangePasswordByCodeDtoValidator>();

            services.AddValidatorsFromAssemblyContaining<ContactUsDtoValidator>();
            services.AddTransient<IValidator<ContactUsClientAddDto>, ContactUsDtoValidator>();


            #endregion

            services.AddTransient<IOrderClient, OrderClient>();
            services.AddTransient<IOrderProvider, OrderProvider>();


            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IHelper, Helper>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ICouponService, CouponService>();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigration>();
            services.AddTransient<IConfigureOptions<SwaggerUIOptions>, SwaggerUIConfiguration>();
            services.AddSwaggerGen();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<ILanguageManager, LanguageManager>();
            services.AddTransient<IAppService, AppService>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IMoreService, MoreService>();
            services.AddTransient<ILangService, LangService>();
            services.AddTransient<ISharedService, SharedService>();
            services.AddTransient<IClientLogicService, ClientLogicService>();
            services.AddTransient<IAdsService, AdsService>();
            services.AddTransient<IDistrictService, DistrictService>();
            services.AddTransient<IPackageService, PackageService>();
            services.AddTransient<IComplaintsService, ComplaintsService>();
            services.AddTransient<IRatesService, RatesService>();
            services.AddTransient<IProhibitedGoodsService, ProhibitedGoodsService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IPaymentsService, PaymentsService>();
            services.AddTransient<IMyFatoorah, MyFatoorah>();


            #region DashBoard
            services.AddTransient<ISettingServices, SettingServices>();
            services.AddTransient<IContactUsServices, ContactUsServices>();
            services.AddTransient<ICoponServices, CoponServices>();
            services.AddTransient<IHomeServices, HomeServices>();
            services.AddTransient<INotificationServices, NotificationServices>();
            services.AddTransient<IAdminServices, AdminServices>();
            services.AddTransient<DashBoardInterface.IUserServices, DashBoardImplementation.UserServices>();
            services.AddTransient<ISocialMediaServices, SocialMediaServices>();
            services.AddTransient<IQuestionServices, QuestionServices>();
            services.AddTransient<ISliderServices, SliderServices>();
            services.AddTransient<ICityServices, CityServices>();
            services.AddTransient<IRegionServices, RegionServices>();
            #endregion
        }


        public static void AddCorsServices(this IServiceCollection services)
        {
            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        builder =>
            //        {
            //            builder.WithOrigins("https://localhost:44306/")
            //            .AllowAnyHeader()
            //            .AllowAnyMethod()
            //            .AllowCredentials();
            //        });
            //});
        }

        public static void AddSessionServices(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);//You can set Time   
            });
        }

        public static void AddController(this IServiceCollection services)
        {
            services.AddControllers();
        }


        public static void AddFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<IStartup>();
        }

    }
}
