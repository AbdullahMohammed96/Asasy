using Asasy.Domain.Entities.AdditionalTables;
using Asasy.Domain.Entities.Advertsments;
using Asasy.Domain.Entities.AsasyPackages;
using Asasy.Domain.Entities.Chat;
using Asasy.Domain.Entities.Follow;
using Asasy.Domain.Entities.Reports;
using Asasy.Domain.Entities.SettingTables;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asasy.Domain.Entities.UserTables
{
    public class ApplicationDbUser : IdentityUser
    {
        public ApplicationDbUser()
        {
            ContactUs = new HashSet<ContactUs>();
            NotifyClient = new HashSet<NotifyUser>();
            DeviceId = new HashSet<DeviceId>();
            Sender = new HashSet<Messages>();
            Receiver = new HashSet<Messages>();
            ConnectUser = new HashSet<ConnectUser>();
            Orders = new HashSet<Order>();
            OrdersP = new HashSet<Order>();

            ReportUsers = new HashSet<Report>();
            ReportProviders = new HashSet<Report>();


            FollowUsers = new HashSet<Asasy.Domain.Entities.Follow.Follow>();
            FollowProviders = new HashSet<Asasy.Domain.Entities.Follow.Follow>();

            RatesUsers = new HashSet<UserRates>();
            RatesProviders = new HashSet<UserRates>();


            CommentsAds = new HashSet<CommentAds>();
            UserPackages = new HashSet<AsasyUserPackage>();
            Favorites = new HashSet<Favorites>();
            Complaints = new HashSet<Complaints>();
            EmployeeComplaints = new HashSet<Complaints>();


            Ads = new HashSet<AdvertsmentDetails>();
            Payments = new HashSet<Payments>();
        }
        public bool ActiveCode { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int Code { get; set; } = 1234;
        public string ShowPassword { get; set; } = "";
        public string Lang { get; set; } = "ar";  //اللغه هتكون عند اليوزر وتكون عربى فى الاول - وتتغير بسيرفس
        public DateTime PublishDate { get; set; } = DateTime.Now;
        //تم اضافته لتعامل مع السيرفس اما 
        //UuserName  ده هنساويه بال email
        public string user_Name { get; set; } //= first_name + " " + last_name;
        public int TypeUser { get; set; } //Client = 1  //deleget = 2 //org_delget = 3//Admin = 4

        public bool CloseNotify { get; set; } = false; //تفعيل الاشعار
        public string ImgProfile { get; set; }

        // for billing
        //[Required(ErrorMessage = "من فضلك ادخل اسم الشارع")]
        public string Street { get; set; } = "";
        //[Required(ErrorMessage = "من فضلك ادخل اسم المدينة")]
        public string City { get; set; } = "";
        //[Required(ErrorMessage = "من فضلك ادخل اسم المنطقة")]
        public string State { get; set; }  = "";
        //[RegularExpression("[A-Z]{2}", ErrorMessage = "ادخل كود الدولة بشكل صحيح مثال : SA")]
        public string Country { get; set; } = "";
        //[Required(ErrorMessage = "من فضلك ادخل الرمز البريدي")]
        public string PostCode { get; set; } = "";
        public string Lat { get; set; } = "";
        public string Lng { get; set; } = "";
        public string Location { get; set; } = "";

        public string CompanyName { get; set; } = "";


        public string PhoneCode { get; set; }
        public double Wallet { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastAppearance { get; set; }
        //Relation

        //ContactUs >> user  m>> o
        public virtual ICollection<ContactUs> ContactUs { get; set; }
        public virtual ICollection<Favorites> Favorites { get; set; }

        //DevieId >> user  m>> o
        public virtual ICollection<DeviceId> DeviceId { get; set; }
        //notifyclient >> user  m>> o
        public virtual ICollection<NotifyUser> NotifyClient { get; set; }
        //notifyDelegt >> user m>> o
        //public virtual ICollection<NotifyDelegt> NotifyDelegt { get; set; }


        // Message
        public virtual ICollection<Messages> Sender { get; set; }
        public virtual ICollection<Messages> Receiver { get; set; }

        public virtual ICollection<ConnectUser> ConnectUser { get; set; }
        public virtual ICollection<CommentAds> CommentsAds { get; set; }
       
        public virtual ICollection<AsasyUserPackage> UserPackages { get; set; }

        [InverseProperty(nameof(Asasy.Domain.Entities.SettingTables.Complaints.User))]
        public virtual ICollection<Complaints> Complaints { get; set; }
        [InverseProperty(nameof(Asasy.Domain.Entities.SettingTables.Complaints.Employee))]
        public virtual ICollection<Complaints> EmployeeComplaints { get; set; }
        // Orders
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Order> OrdersP { get; set; }


        [InverseProperty(nameof(Report.User))]
        public virtual ICollection<Report> ReportUsers { get; set; }
        [InverseProperty(nameof(Report.Provider))]
        public virtual ICollection<Report> ReportProviders { get; set; }


        [InverseProperty(nameof(Asasy.Domain.Entities.Follow.Follow.User))]
        public virtual ICollection<Asasy.Domain.Entities.Follow.Follow> FollowUsers { get; set; }
        [InverseProperty(nameof(Asasy.Domain.Entities.Follow.Follow.Provider))]
        public virtual ICollection<Asasy.Domain.Entities.Follow.Follow> FollowProviders { get; set; }


        [InverseProperty(nameof(UserRates.User))]
        public virtual ICollection<UserRates> RatesUsers { get; set; }
        [InverseProperty(nameof(UserRates.Provider))]
        public virtual ICollection<UserRates> RatesProviders { get; set; }


        public virtual ICollection<AdvertsmentDetails> Ads { get; set; }
        public virtual ICollection<Payments> Payments { get; set; }



    }
}
