using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ClientLogicDto
{
    public class AdsDetailsToUpdateDto
    {
        public int adsId { get; set; }
        public string title { get; set; }
        public int regionId { get; set; }
        public int cityId { get; set; }
        public int districtId { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string location { get; set; }
        public double price { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public int statusProduct { get; set; }
        public string description { get; set; }
        public List<AdImagesDto> images { get; set; }
        //public List<IFormFile> newImages { get; set; }
        public string phoneNumber { get; set; }
        public string phoneCode { get; set; }
        public bool showPhone { get; set; }

    }
    public class AdImagesDto
    {
        public int imageId { get; set; }
        public string image { get; set; }
    }

}
