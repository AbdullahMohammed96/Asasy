using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ClientLogicDto
{
    public class AdsListDto
    {
        public int adsId { get; set; }
        public string description { get; set; }
        public bool isSpecial { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public List<string> images { get; set; }
        public string providerName { get; set; }
        public string date { get; set; }
        public string cityName { get; set; }
        public string districtName { get; set; }
        public double price { get; set; }
        public bool isFavorite { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string location { get; set; }

    }
}
