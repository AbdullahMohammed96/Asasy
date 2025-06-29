using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.AuthApiDTO
{
    public class LoginDto
    {
        [RegularExpression("^(5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "أدخل رقم جوال صحيح")]
        public string phone { get; set; }
        public string phoneCode { get; set; }
        //public string password { get; set; }

        public string lang { get; set; } = "ar";
        public string deviceType { get; set; }
        public string deviceId { get; set; }
        public string projectName { get; set; } = "Project Name";

        
    }
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator(IStringLocalizer<LoginDto> localizer)
        {

            RuleFor(x => x.phone).NotEmpty()
                .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "phone"), Helper.getFileTranslate(Lang.Translate.en, "phone")));

            //RuleFor(x => x.password).NotEmpty()
            //    .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "password"), Helper.getFileTranslate(Lang.Translate.en, "password")))
            //           .Length(6, 100)
            //           .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "length"), Helper.getFileTranslate(Lang.Translate.en, "length")));



        }

    }
}
