using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Users
{
    public class CreateUserViewModel
    {


        [Required(ErrorMessage = "هذا الحقل مطلوب")]

        public string Name { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]

        public IFormFile Image { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [RegularExpression("^(5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "أدخل رقم جوال صحيح")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]

        public string Code { get; set; } = "+966";
        [EmailAddress(ErrorMessage = "هذا الحقل لا يقبل الا ايميل")]
        [RegularExpression("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]{2,}@[a-zA-Z0-9]+(?:[a-zA-Z0-9-]*[a-zA-Z0-9]\\.)+[a-zA-Z]{2,}$", ErrorMessage = "فضلا أدخل البريد الالكتروني بطريقه صحيحه")]
        public string? Email { get; set; }

    }
}
