using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.UserIndexDto
{
    public class UserIndexDto
    {
        public string Id { get; set; }
        public string User_Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string ImgProfile { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
    }
}
