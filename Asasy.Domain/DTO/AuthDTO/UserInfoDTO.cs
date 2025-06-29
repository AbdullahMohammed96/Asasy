using Asasy.Domain.DTO.CategoryDto;
using System;
using System.Collections.Generic;

namespace Asasy.Domain.DTO.AuthDTO
{
    public class UserInfoDTO
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string lang { get; set; }
        public bool closeNotify { get; set; }
        public bool status { get; set; }
        public string imgProfile { get; set; }
        public string token { get; set; }
        // public DateTime? expiration { get; set; }
        public int typeUser { get; set; }
        public int code { get; set; }
        public bool ActiveCode { get; set; }
        public string location { get; set; }
        public string companyName { get; set; }
        public string registerDate { get; set; }
    }
}
