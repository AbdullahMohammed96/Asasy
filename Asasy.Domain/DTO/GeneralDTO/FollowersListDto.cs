using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.GeneralDTO
{
    public class FollowersListDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string userId { get; set; }
        public string img { get; set; }
        public string location { get; set; }
    }
}
