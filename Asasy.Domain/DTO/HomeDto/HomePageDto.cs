using Asasy.Domain.DTO.CategoryDto;
using Asasy.Domain.DTO.ProductDto;
using Asasy.Domain.DTO.RegionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.HomeDto
{
    public class HomePageDto
    {
        public IReadOnlyList<CategoryListDto> CategoryListDtos { get; set; }
        public IReadOnlyList<CategoryListDto> CategoryListSideMenuDtos { get; set; }
        public IReadOnlyList<RegionListDto> RegionListDtos { get; set; }
        public IReadOnlyList<ProductListDto> ProductListDtos { get; set; }

    }
}
