using Microsoft.AspNetCore.Http;
using System;

namespace Asasy.Domain.DTO.AuthDTO
{
    public class UserInfoAddDTO
    {
        public IFormFile imgProfile { get; set; }

        public string userName { get; set; }
        public string companyName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string phoneCode { get; set; }
        public int code { get; set; }
        public string deviceId { get; set; }
        public string deviceType { get; set; }
        /// <summary>
        /// for title of notification
        /// </summary>
        public string projectName { get; set; }

        /// <summary>
        ///ar or en
        /// </summary>

        public string lang { get; set; } = "ar";
    }
}
