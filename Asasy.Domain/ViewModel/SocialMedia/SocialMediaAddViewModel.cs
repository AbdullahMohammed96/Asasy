using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.SocialMedia
{
    public class SocialMediaAddViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string NameAr { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string NameEn { get; set; }
        [Required(ErrorMessage = "من فضلك اختر الصورة")]
        public IFormFile Img { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string Url { get; set; }
    }
}
