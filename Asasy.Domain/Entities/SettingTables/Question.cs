using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.SettingTables
{
    public class Question
    {
        public int Id { get; set; }

        public string QuestionAr { get; set; }

        public string AnswerAr { get; set; }
        public string QuestionEn { get; set; }
        public string AnswerEn { get; set; }
        public bool IsActive { get; set; }
        public string ChangeLangQuestion(string lang = "ar")
        {
            return AAITHelper.HelperMsg.creatMessage(lang, QuestionAr, QuestionEn);
        }
        public string ChangeLangAnswer(string lang = "ar")
        {
            return AAITHelper.HelperMsg.creatMessage(lang, AnswerEn, AnswerEn);
        }
    }
}
