using Asasy.Domain.Entities.AsasyPackages;
using Asasy.Domain.Entities.Categories;
using Asasy.Domain.Entities.Cities_Tables;
using Asasy.Domain.Entities.Follow;
using Asasy.Domain.Entities.Reports;
using Asasy.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.Advertsments
{
    public class AdvertsmentDetails
    {
        public AdvertsmentDetails()
        {
            AdvertsmentImages = new HashSet<AdvertsmentImages>();
            Reports = new HashSet<Report>();
            Follows = new HashSet<Asasy.Domain.Entities.Follow.Follow>();
            CommentsAds = new HashSet<CommentAds>();
            UserPackages = new HashSet<AsasyUserPackage>();
            Favorites = new HashSet<Favorites>();
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RegionId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int StatusProduct { get; set; } // new or old
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Phone { get; set; }
        public string PhoneCode { get; set; }
        public bool ShowPhone { get; set; }
        public bool IsSpecial { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationDbUser User { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
        [ForeignKey(nameof(SubCategoryId))]
        public virtual SubCategories SubCategory { get; set; }
        public virtual ICollection<AdvertsmentImages>? AdvertsmentImages { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Asasy.Domain.Entities.Follow.Follow> Follows { get; set; }

        public virtual ICollection<CommentAds> CommentsAds { get; set; }
        public virtual ICollection<AsasyUserPackage> UserPackages { get; set; }
        public virtual ICollection<Favorites> Favorites { get; set; }

    }
}
