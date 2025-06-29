using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Ads
{
    public class AdEditViewModel
    {
        public int AdsId { get; set; }
        public int RegionId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        [Required(ErrorMessage = "فضلا هذا الحقل مطلوب")]
        public string Title { get; set; }
        [Required(ErrorMessage = "فضلا هذا الحقل مطلوب")]
        public double Price { get; set; }
        public int StatusProduct { get; set; } // new or old
        [Required(ErrorMessage = "فضلا هذا الحقل مطلوب")]
        public string Description { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Location { get; set; }
        [Required(ErrorMessage ="فضلا هذا الحقل مطلوب")]
        [RegularExpression("^(5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "أدخل رقم جوال صحيح")]
        public string Phone { get; set; }
        public string PhoneCode { get; set; } = "+966";
        public bool ShowPhone { get; set; }
        public List<string> Images { get; set; }
        public string oldImages { get; set; }
        public List<IFormFile> NewImages { get; set; }



    }
}
