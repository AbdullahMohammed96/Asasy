using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ClientLogicDto
{
    public class FollowsProviderListDto
    {
        [JsonIgnore]
        public int followId { get; set; }
        public string providerName { get; set; }
        public string providerImage { get; set; }
        public bool isOnline { get; set; }
        public string joiningDate { get; set; }
        public string lastAppearance { get; set; }
        [JsonIgnore]
        public double sumRate { get; set; }
        [JsonIgnore]
        public int countRate { get; set; }
        public double averageRate => sumRate != 0 && countRate != 0 ? Math.Round(sumRate / countRate, 1) : 0;
        public string providerId { get; set; }
    }
}
