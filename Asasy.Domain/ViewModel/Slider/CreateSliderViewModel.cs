using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Slider
{
    public class CreateSliderViewModel
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string TitleAr { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string TitleEn { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string DescriptionAr { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string DescriptionEn { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [RegularExpression("https?:\\/\\/(?:www\\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\\.[^\\s]{2,}|www\\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\\.[^\\s]{2,}|https?:\\/\\/(?:www\\.|(?!www))[a-zA-Z0-9]+\\.[^\\s]{2,}|www\\.[a-zA-Z0-9]+\\.[^\\s]{2,}",ErrorMessage ="ادخل الرابط بشكل صحيح")]
        public string Link { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public IFormFile Image { get; set; }

    }
}
