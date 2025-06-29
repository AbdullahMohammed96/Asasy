using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Districts
{
    public class EditDistrictViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل اسم الحي بالعربية ")]
        public string NameAr { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل اسم الحي بالانجليزية")]
        public string NameEn { get; set; }
        [Required(ErrorMessage = "من فضلك اختر المدينه")]
        [Range(1, int.MaxValue, ErrorMessage = "من فضلك اختر المدينه")]
        public int CityId { get; set; }
    }
}
