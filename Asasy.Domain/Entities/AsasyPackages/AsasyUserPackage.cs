using Asasy.Domain.Entities.Advertsments;
using Asasy.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.AsasyPackages
{
    public class AsasyUserPackage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AdsId { get; set; }
        public string PackageNameAr { get; set; }
        public string PackageNameEn { get; set; }
        public string PackageDescriptionAr { get; set; }
        public string PackageDescriptionEn { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsPayed { get; set; }
        public int TypePay { get; set; }
        public double Price { get; set; }
        public int CountDayes { get; set; }
        public int PackageId { get; set; }
        public int StatusReview { get; set; } // waiting || accept || refuse

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationDbUser User { get; set; }
        [ForeignKey(nameof(AdsId))]
        public virtual AdvertsmentDetails Ads { get; set; }

    }
}
