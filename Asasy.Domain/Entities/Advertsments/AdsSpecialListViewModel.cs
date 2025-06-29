using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.Advertsments
{
    public class AdsSpecialListViewModel
    {
        public int Id { get; set; }
        public string User { get; set; }
        public int AdsId { get; set; }
        public string AdsTitle { get; set; }
        public string PackageNameAr { get; set; }
        public string PackageNameEn { get; set; }
        public string PackageDescriptionAr { get; set; }
        public string PackageDescriptionEn { get; set; }
        public string SubscriptionDate { get; set; }
        public string EndDate { get; set; }
        public bool IsPayed { get; set; }
        public double Price { get; set; }
    }
}
