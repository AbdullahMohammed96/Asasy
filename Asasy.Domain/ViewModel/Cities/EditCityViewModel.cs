using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Cities
{
    public class EditCityViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل اسم المدينة بالعربية ")]
        public string NameAr { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل اسم المدينة بالانجليزية")]
        public string NameEn { get; set; }
        [Required(ErrorMessage = "من فضلك اختر المنطقه")]
        [Range(1, int.MaxValue, ErrorMessage = "من فضلك اختر المنطقه")]
        public int RegionId { get; set; }
    }
}
