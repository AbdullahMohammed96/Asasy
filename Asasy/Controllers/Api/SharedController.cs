using AAITHelper.Enums;
using AAITHelper;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.DTO.AuthDTO;
using Asasy.Domain.DTO;
using Asasy.Service.Api.Contract.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Asasy.Domain.DTO.Shared;
using Asasy.Service.Api.Implementation.FilterValidation;

namespace Asasy.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "SharedAPI")]
    [PermessionFilterAttribute]
    [ApiController]
    public class SharedController : ControllerBase
    {
        private readonly ISharedService _sharedService;

        public SharedController(ISharedService sharedService)
        {
            _sharedService = sharedService;
        }

        /// <summary>
        /// List Of Main Categories; Which contains only sub-categories.
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<CategoriesListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Shared.MaiCategories)]
        [AllowAnonymous]
        public async Task<IActionResult> MaiCategories()
        {
            var data = await _sharedService.Categories();

            return Ok(data);
        }


        /// <summary>
        /// List Of SubCategories.
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<SubCategoriesListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Shared.SubCategories)]
        [AllowAnonymous]
        public async Task<IActionResult> SubCategories(int categoryId)
        {
            var data = await _sharedService.SubCategoriesByCategory(categoryId);

            return Ok(data);
        }

        /// <summary>
        /// List Of Regions; Which contains only cities
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<RegionListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Shared.ListOfRegions)]
        [AllowAnonymous]
        public async Task<IActionResult> ListOfRegions()
        {
            var data = await _sharedService.Regions();

            return Ok(data);
        }


        /// <summary>
        /// List Of Cities by RegionId; Which contains only districts
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<CitiesListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Shared.ListOfCitiesByRegion)]
        [AllowAnonymous]
        public async Task<IActionResult> ListOfCitiesByRegion(int regionId)
        {
            var data = await _sharedService.CitiesByRegion(regionId);

            return Ok(data);
        }


        /// <summary>
        /// List Of District by cityId 
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<CitiesListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Shared.ListOfDistrictsByCity)]
        [AllowAnonymous]
        public async Task<IActionResult> ListOfDistrictsByCity(int cityId)
        {
            var data = await _sharedService.DistrictsByCity(cityId);

            return Ok(data);
        }
        [ProducesResponseType(typeof(BaseResponseDto<List<CitiesListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Shared.Packages)]
        [AllowAnonymous]
        public async Task<IActionResult> Packages()
        {
            var data = await _sharedService.Packages();

            return Ok(data);
        }
        [ProducesResponseType(typeof(BaseResponseDto<List<CitiesListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.Shared.GetPackageDetails)]
        [AllowAnonymous]
        public async Task<IActionResult> GetPackageDetails(int packageId)
        {
            var data = await _sharedService.GetPackageDeails(packageId);

            return Ok(data);
        }
    }
}
