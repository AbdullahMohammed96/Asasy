using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.More
{
    public class ProhibitedGoodsListDto
    {
        [JsonIgnore]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
