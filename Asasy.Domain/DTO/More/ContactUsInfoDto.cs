using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.More
{
    public class ContactUsInfoDto
    {
        public string contactUsText { get; set; }
        public string   lat { get; set; }
        public string   lng { get; set; }
        public string location { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
