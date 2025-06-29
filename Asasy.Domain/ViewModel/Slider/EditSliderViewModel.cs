using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Slider
{
    public class EditSliderViewModel
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "هذا الحقل مطلوب")]
        //public string SliderName { get; set; }
        //public string CurrentImage { get; set; }
        //public IFormFile NewImage { get; set; }
        //[Required(ErrorMessage = "هذا الحقل مطلوب")]
        //public string Url { get; set; }
        //[Required(ErrorMessage = "هذا الحقل مطلوب")]
        //public DateTime ExpireDate { get; set; }


        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string TitleAr { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string TitleEn { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string DescriptionAr { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string DescriptionEn { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [RegularExpression("https?:\\/\\/(?:www\\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\\.[^\\s]{2,}|www\\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\\.[^\\s]{2,}|https?:\\/\\/(?:www\\.|(?!www))[a-zA-Z0-9]+\\.[^\\s]{2,}|www\\.[a-zA-Z0-9]+\\.[^\\s]{2,}", ErrorMessage = "ادخل الرابط بشكل صحيح")]
        public string Link { get; set; }
        public IFormFile NewImage { get; set; }
        public string Image { get; set; }


    }
}
