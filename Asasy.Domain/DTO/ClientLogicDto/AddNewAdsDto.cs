using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ClientLogicDto
{
    public class AddNewAdsDto
    {
        public int regionId { get; set; }
        public int cityId { get; set; }
        public int districtId { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public string title { get; set; }
        public double price { get; set; }
        /// <summary>
        /// StatusProduct
        /// {
        ///     New=1,
        ///     Old=2
        /// }
        /// </summary>
        public int statusProduct { get; set; } 
        public string description { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string location { get; set; }
        public string phone { get; set; }
        public string phoneCode { get; set; }
        public bool showPhone { get; set; }

        public List<IFormFile> images { get; set; }

    }
}
