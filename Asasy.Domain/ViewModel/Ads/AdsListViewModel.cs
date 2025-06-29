using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Ads
{
    public class AdsListViewModel
    {
        public int AdsId { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string StatusProduct { get; set; } // new or old
        public string Description { get; set; }
        public string CreationDate { get; set; }       
        public bool IsActive { get; set; }
        public string Phone { get; set; }
        public string PhoneCode { get; set; }
        public bool IsSpecial { get; set; }
    }
}
