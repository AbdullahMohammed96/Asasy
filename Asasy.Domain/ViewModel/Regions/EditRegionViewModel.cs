using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Regions
{
    public class EditRegionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل اسم المنطقه بالعربية ")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم المنطقه بالانجليزية")]
        public string NameEn { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "من فضلك اختر مدينتك")]
        //public int CityId { get; set; }
    }
}
