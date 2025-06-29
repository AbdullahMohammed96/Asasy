using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ClientLogicDto
{
    public class SlidersListDto
    {
        [JsonIgnore]
        public int id { get; set; }
        public string image { get; set; }
        public string link { get; set; }
    }
}
