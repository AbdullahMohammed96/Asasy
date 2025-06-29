using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Districts
{
    public class CreateDistrictViewModel
    {
        [Required(ErrorMessage ="فضلا قم بادخال اسم الحي بالعربيه")]
        public string NameAr { get; set; }
        [Required(ErrorMessage = "فضلا قم بادخال اسم الحي بالانجليزيه")]
        public string NameEn { get; set; }
        [Required(ErrorMessage = "فضلا اختر المدينه")]
        [Range(1, int.MaxValue, ErrorMessage = "من فضلك اختر المدينه")]
        public int CityId { get; set; }
    }
}
