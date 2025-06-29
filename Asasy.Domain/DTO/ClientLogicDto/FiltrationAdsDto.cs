using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ClientLogicDto
{
    public class FiltrationAdsDto
    {
        public int regionId { get; set; }
        /// <summary>
        /// 08-07-2024
        /// </summary>
        public DateTime from { get; set; }
        /// <summary>
        /// 08-10-2024
        /// </summary>
        public DateTime to { get; set; }
        public bool isLatest { get; set; }
        public bool isContainImages { get; set; }
        public int categoryId { get; set; }
        public List<int> subCategories { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;

    }
}
