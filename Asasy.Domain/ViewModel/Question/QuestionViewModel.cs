using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Question
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="هذا الحقل مطلوب")]
        public string QuestionAr { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string AnswerAr { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string QuestionEn { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string AnswerEn { get; set; }
        public bool IsActive { get; set; }
    }
}
