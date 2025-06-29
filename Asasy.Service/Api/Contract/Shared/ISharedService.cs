using Asasy.Domain.DTO;
using Asasy.Domain.DTO.RegionDto;
using Asasy.Domain.DTO.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Contract.Shared
{
    public interface ISharedService
    {

        Task<BaseResponseDto<List<Domain.DTO.RegionDto.RegionListDto>>> Regions();
        Task<BaseResponseDto<List<CitiesListDto>>> CitiesByRegion(int RegionId);
        Task<BaseResponseDto<List<DistrictListDto>>> DistrictsByCity(int CityId);

        Task<BaseResponseDto<List<CategoriesListDto>>> Categories();
        Task<BaseResponseDto<List<SubCategoriesListDto>>> SubCategoriesByCategory(int CategoryId);

        Task<BaseResponseDto<List<PackagesListDto>>> Packages();
        Task<BaseResponseDto<PackagesListDto>> GetPackageDeails(int PackageId);
    }
}
