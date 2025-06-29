using Asasy.Domain.Entities;
using Asasy.Domain.Entities.AdditionalTables;
using Asasy.Domain.Entities.Advertsments;
using Asasy.Domain.Entities.AsasyPackages;
using Asasy.Domain.Entities.Categories;
using Asasy.Domain.Entities.Chat;
using Asasy.Domain.Entities.Cities_Tables;
using Asasy.Domain.Entities.Copon;
using Asasy.Domain.Entities.Follow;
using Asasy.Domain.Entities.Reports;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Entities.UserTables;
using Asasy.Persistence.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Asasy.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationDbUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LogExption> LogExption { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<DeviceId> DeviceIds { get; set; }
        public DbSet<NotifyUser> NotifyUsers { get; set; }
        public DbSet<NotifyDelegt> NotifyDelegts { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Copon> Copon { get; set; }
        public DbSet<CoponUsed> CoponUsed { get; set; }
        public DbSet<ConnectUser> ConnectUser { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderInfo> OrderInfos { get; set; }
        public DbSet<HistoryNotify> HistoryNotify { get; set; }
        public DbSet<SocialMedia> socialMedias { get; set; }
        public DbSet<Question> Questions { get; set; }
        //public DbSet<Slider> Sliders { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        //new
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AdvertsmentDetails> AdvertsmentDetails { get; set; }
        public DbSet<AdvertsmentImages> AdvertsmentImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategories> SubCategories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<AsasyPackage> Packages { get; set; }
        public DbSet<AsasyUserPackage> UserPackages { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<Complaints> Complaints { get; set; }
        public DbSet<CommentAds> CommentAds { get; set; }
        public DbSet<ReplaiesComment> ReplaiesComments { get; set; }
        public DbSet<UserRates> UserRates { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<ProhibitedGoods> ProhibitedGoods { get; set; }
        public DbSet<Chats> Chats { get; set; }
        public DbSet<Payments> PaymentUsers { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationDbUser>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<ContactUs>().HasQueryFilter(c => !c.IsDeleted);

            builder.Entity<Slider>().HasQueryFilter(c => !c.IsDelete);
            builder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<SubCategories>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<AdvertsmentDetails>().HasQueryFilter(c => !c.IsDelete);
            builder.Entity<Region>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<City>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<District>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<Complaints>().HasQueryFilter(c => !c.IsDelete);


            builder.Entity<Follow>().Property(c => c.AdsId).IsRequired(false);
            builder.Entity<Follow>().Property(c => c.ProviderId).IsRequired(false);
            builder.Entity<Complaints>().Property(c => c.EmployeeId).IsRequired(false);
            builder.Entity<Report>().Property(c => c.AdsId).IsRequired(false);
            builder.Entity<Report>().Property(c => c.ProviderId).IsRequired(false);
            //builder.Seed();

            builder.Entity<ApplicationDbUser>()
            .HasMany(c => c.Sender)
            .WithOne(o => o.Sender)
            .HasForeignKey(o => o.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationDbUser>()
                       .HasMany(c => c.Receiver)
                       .WithOne(o => o.Receiver)
                       .HasForeignKey(o => o.ReceiverId)
                       .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationDbUser>()
                    .HasMany(c => c.Orders)
                    .WithOne(o => o.User)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
        }

    }

}
