using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.AuthDTO
{
    public class NotifyListDto
    {
        public int id { get; set; }
        public string text { get; set; }
        public string date { get; set; }
        public int? type { get; set; }
        public int? adsId { get; set; }
    }
}
