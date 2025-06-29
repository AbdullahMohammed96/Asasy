using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ClientLogicDto
{
    public class AdsDetailsDto
    {
        public int adsId { get; set; }
        public string providerId { get; set; }
        public List<string> images { get; set; }
        public bool  isFavorite { get; set; }
        public bool  isFollow { get; set; }
        public bool  isSpecial { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public int categoryId { get; set; }
        public string subCategory { get; set; }
        public int subCategoryId { get; set; }
        public double price { get; set; }
        public string lat { get; set; }
        public string  lng{ get; set; }
        public string  location{ get; set; }
        public string phoneNumber { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public string statusProduct { get; set; }
        public string userImage { get; set; }
        public string userName { get; set; }
        public bool showPhone { get; set; }
        [JsonIgnore]
        public double sumRate { get; set; }
        [JsonIgnore]
        public int countRate { get; set; }
        public double averageRate => sumRate != 0 && countRate != 0 ? Math.Round(sumRate / countRate, 1) : 0;
        public List<AdsCommentDto> comments { get; set; }
        public List<AdsListDto> similarAds { get; set; }
        public int chatId { get; set; }
        public string createAt { get; set; }

    }

    public class AdsCommentDto
    {
        //[JsonIgnore]
        public int commentId { get; set; }
        public string userName { get; set; }
        public string comment { get; set; }
        public string userImage { get; set; }
        public string date { get; set; }
        public List<ReplaiesCommentDto> replaies { get; set; }
    }


    public class ReplaiesCommentDto
    {
        //[JsonIgnore]
        public int commentId { get; set; }
        public string userName { get; set; }
        public string comment { get; set; }
        public string userImage { get; set; }
        public string date { get; set; }
    }

}
