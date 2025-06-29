using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.AuthDTO
{
    public class SendCodeToPhoneDto
    {
        [RegularExpression("^(5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "أدخل رقم جوال صحيح")]
        public string phoneNumber { get; set; }
        public string phoneCode { get; set; }
    }
}
