using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ChatDTO
{
    public class AdInfoDto
    {
        public int  adId { get; set; }
        public string creationDate { get; set; }
        public string adDate { get; set; }
        public string image { get; set; }
        public string  title { get; set; }
        public double price { get; set; }

    }

}
