using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Package
{
    public class AddNewPackageViewModel
    {
        [Required(ErrorMessage ="فضلا ادخل الاسم بالعربيه")]
        public string NameAr { get; set; }
        [Required(ErrorMessage = "فضلا ادخل الاسم بالانجليزيه")]
        public string NameEn { get; set; }
        [Required(ErrorMessage = "فضلا ادخل الوصف بالعربيه")]
        public string DescriptionAr { get; set; }
        [Required(ErrorMessage = "فضلا ادخل الوصف بالانجليزيه")]
        public string DescriptionEn { get; set; }
        [Required(ErrorMessage = "فضلا السعر")]
        [Range(1, Int32.MaxValue, ErrorMessage = "يجب الا يقل السعر عن 1")]
        public double Price { get; set; }
        [Required(ErrorMessage = "فضلا المده")]
        [Range(1, Int32.MaxValue, ErrorMessage = "يجب الا تقل المده عن يوم")]
        public int CountDayes { get; set; }
    }
}
