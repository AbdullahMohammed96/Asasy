using System.ComponentModel.DataAnnotations;

namespace Asasy.Domain.ViewModel.Settings
{
    public class SettingEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [MinLength(10, ErrorMessage = "ادخل رقم الهاتف صحيح ولا يقل عن 10 أرقام")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string CondtionsArClient { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string CondtionsEnClient { get; set; }
        //[Required(ErrorMessage = "هذا الحقل مطلوب")]
        //public string CondtionsArDelegt { get; set; }
        //[Required(ErrorMessage = "هذا الحقل مطلوب")]
        //public string CondtionsEnDelegt { get; set; }
        //[Required(ErrorMessage = "هذا الحقل مطلوب")]
        //public string AboutUsArClient { get; set; }
        //[Required(ErrorMessage = "هذا الحقل مطلوب")]
        //public string AboutUsEnClient { get; set; }
        //[Required(ErrorMessage = "هذا الحقل مطلوب")]
        //public string AboutUsArDelegt { get; set; }
        //[Required(ErrorMessage = "هذا الحقل مطلوب")]
        //public string AboutUsEnDelegt { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string FooterAr { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string FooterEn { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string SenderName { get; set; } = "test";
        public string UserNameSms { get; set; } = "test";
        public string PasswordSms { get; set; } = "test";
        public string ApplicationId { get; set; }
        public string SenderId { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [RegularExpression("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]{2,}@[a-zA-Z0-9]+(?:[a-zA-Z0-9-]*[a-zA-Z0-9]\\.)+[a-zA-Z]{2,}$", ErrorMessage = "فضلا أدخل البريد الالكتروني بطريقه صحيحه")]
        public string Email { get; set; }

        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Location { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public double CommissionApp { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string PolicyAr { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string PolicyEn { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string DiscountSystemAr { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string DiscountSystemEn { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string ContactUsAr { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string ContactUsEn { get; set; }


    }
}
