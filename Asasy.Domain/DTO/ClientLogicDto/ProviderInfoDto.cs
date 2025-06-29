using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ClientLogicDto
{
    public class ProviderInfoDto
    {
        public string providerId { get; set; }
        public string providerName { get; set; }
        public bool isOnline { get; set; }
        public string imageProfile { get; set; }
        public string lastAppearance { get; set; }
        public string joiningDate { get; set; }
        public bool isFollow { get; set; }
        [JsonIgnore]
        public double sumRate { get; set; }
        [JsonIgnore]
        public int countRate { get; set; }
        public double averageRate => sumRate != 0 && countRate != 0 ? Math.Round(sumRate / countRate, 1) : 0;
        public List<CommentsListDto> comments { get; set; }
        public List<AdsListDto> providerAds { get; set; }
        public int chatId { get; set; }

    }

    public class CommentsListDto
    {
        [JsonIgnore]
        public int commentId { get; set; }
        public string userName { get; set; }
        public string comment { get; set; }
        public double rate { get; set; }
        public string userImage { get; set; }
    }
}
