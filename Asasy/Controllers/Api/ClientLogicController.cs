using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.DTO.Shared;
using Asasy.Domain.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Asasy.Service.Api.Contract.ClientLogic;
using Asasy.Domain.DTO.ClientLogicDto;
using Asasy.Service.Api.Implementation.FilterValidation;

namespace Asasy.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "ClientLogicAPI")]
    [PermessionFilterAttribute]
    [ApiController]
    public class ClientLogicController : ControllerBase
    {

        private readonly IClientLogicService _clientLogicService;

        public ClientLogicController(IClientLogicService clientLogicService)
        {
            _clientLogicService = clientLogicService;
        }

        /// <summary>
        /// List Of Main Categories; Which contains only sub-categories.
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.AddNewAds)]
        public async Task<IActionResult> AddNewAds([FromForm] AddNewAdsDto model)
        {
            var check = await _clientLogicService.AddNewAds(model);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }


        /// <summary>
        /// Subscribe to a package to distinguish the advertisement
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.SubscribeInPackageByWallet)]
        public async Task<IActionResult> SubscribeInPackageByWallet(int adsId, int packageId)
        {
            var check = await _clientLogicService.SubscribeInPackageByWallet(adsId, packageId);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }


     
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.ChargingWallet)]
        public async Task<IActionResult> ChargingWallet(double balance)
        {
            var check = await _clientLogicService.WalletCharging(balance);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }


        /// <summary>
        /// Get List Of Sliders
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<SlidersListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.Slider)]
        [AllowAnonymous]
        public async Task<IActionResult> Slider()
        {
            var check = await _clientLogicService.Sliders();
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }


        /// <summary>
        /// Ads In Home Client
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<AdsListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.Home)]
        [AllowAnonymous]
        public async Task<IActionResult> Home(int pageSize = 10, int pageNumber = 1)
        {
            var check = await _clientLogicService.Home(pageSize, pageNumber);

            return Ok(check);

        }



        /// <summary>
        /// Filtration Ads in home
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<AdsListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.FiltrationAds)]
        [AllowAnonymous]
        public async Task<IActionResult> FiltrationAds([FromForm] FiltrationAdsDto model)
        {
            var check = await _clientLogicService.FiltrationAds(model);

            return Ok(check);

        }


        /// <summary>
        /// Filtration Ads in home
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<AdsListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.AdsByLocation)]
        [AllowAnonymous]
        public async Task<IActionResult> AdsByLocation(string lat, string lng, int pageSize = 10, int pageNumber = 1)
        {
            var check = await _clientLogicService.AdsByLocation(lat,lng, pageSize, pageNumber);

            return Ok(check);

        }

        /// <summary>
        /// List of ads by only { categoryId } or { categoryId and subCategoryId } ot { categoryId and subCategoryId and productName }
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<AdsListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.ListOfAdsByCategoryAndSubCategoryAndTitle)]
        [AllowAnonymous]
        public async Task<IActionResult> ListOfAdsByCategoryAndSubCategoryAndTitle(int categoryId, int subCategoryId, string title, int pageSize = 10, int pageNumber = 1)
        {
            var check = await _clientLogicService.ListOfAdsByCategoryAndSubCategoryAndTitle(categoryId, subCategoryId, title,pageSize,pageNumber);

            return Ok(check);

        }


        /// <summary>
        /// Get Ad Details for client
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<AdsDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.AdDetails)]
        [AllowAnonymous]
        public async Task<IActionResult> AdDetails(int adId, int pageSize = 10, int pageNumber = 1)
        {
            var check = await _clientLogicService.AdsDetails(adId,pageSize,pageNumber);

            return Ok(check);

        }

        /// <summary>
        /// Add New Comment For Ad
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.AddNewCommentForAd)]
        public async Task<IActionResult> AddNewCommentForAd(int adsId, string comment)
        {
            var check = await _clientLogicService.AddNewCommentForAd(adsId, comment);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }


        /// <summary>
        /// Add New replay to Comment For Ad
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.NewReplayForComment)]
        public async Task<IActionResult> NewReplayForComment(int commentId, string comment)
        {
            var check = await _clientLogicService.AddNewReplayForComment(commentId, comment);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }


        /// <summary>
        /// All Comments For Ads
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<AdsCommentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.AllCommentsForAds)]
        [AllowAnonymous]
        public async Task<IActionResult> AllCommentsForAds(int adId)
        {
            var check = await _clientLogicService.CommentsAds(adId);

            return Ok(check);

        }

        /// <summary>
        /// Add Ad To Favorite
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.AddAdToFavorite)]
        public async Task<IActionResult> AddAdToFavorite(int adsId)
        {
            var check = await _clientLogicService.AddAdsToFavorite(adsId);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }

        /// <summary>
        /// Remove Ad From Favorite
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.RemoveAdFromFavorite)]
        public async Task<IActionResult> RemoveAdFromFavorite(int adsId)
        {
            var check = await _clientLogicService.RemoveAdFromFavorite(adsId);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }


        /// <summary>
        /// Get List of favorite user
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<SlidersListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.FavoriteList)]
        public async Task<IActionResult> FavoriteList()
        {
            var check = await _clientLogicService.FavoriteList();

            return Ok(check);

        }


        /// <summary>
        /// Add new report for ad by client
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.ReportAd)]
        public async Task<IActionResult> ReportAd([FromForm] ReportAdsDto model)
        {
            var check = await _clientLogicService.ReportAds(model);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }


        /// <summary>
        /// Add new report for provider by client
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.ReportProvider)]
        public async Task<IActionResult> ReportAd([FromForm] ReportProviderDto model)
        {
            var check = await _clientLogicService.ReportProvider(model);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }



        /// <summary>
        /// Follow ad by client
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.FollowAd)]
        public async Task<IActionResult> FollowAd(int adId)
        {
            var check = await _clientLogicService.FollowAd(adId);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }


        /// <summary>
        /// Follow provider by client
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.FollowProvider)]
        public async Task<IActionResult> FollowProvider(string providerId)
        {
            var check = await _clientLogicService.FollowProvider(providerId);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }


        /// <summary>
        /// UnFollow  ad by client
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.UnFollowAd)]
        public async Task<IActionResult> UnFollowAd(int adId)
        {
            var check = await _clientLogicService.UnFollowAd(adId);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }


        /// <summary>
        /// UnFollow provider  by client
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.UnFollowProvider)]
        public async Task<IActionResult> UnFollowProvider(string providerId)
        {
            var check = await _clientLogicService.UnFollowProvider(providerId);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }



        /// <summary>
        /// Get List of Follows Ad
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<FollowsAdsListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.ListOfFollowsAd)]
        public async Task<IActionResult> ListOfFollowsAd()
        {
            var check = await _clientLogicService.FollowsAds();

            return Ok(check);

        }

        /// <summary>
        /// Get List of Follows Provider
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<FollowsProviderListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.ListOfFollowsProvider)]
        public async Task<IActionResult> ListOfFollowsProvider()
        {
            var check = await _clientLogicService.FollowsProvider();

            return Ok(check);

        }

        /// <summary>
        /// Get provider info for client
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<ProviderInfoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.GetProviderInfo)]
        [AllowAnonymous]
        public async Task<IActionResult> GetProviderInfo(string providerId,int PageSize = 10, int PageNumber = 1)
        {
            var check = await _clientLogicService.ProviderInfo(providerId, PageSize,PageNumber);

            return Ok(check);

        }

        /// <summary>
        /// Add New Rate For Provider by client
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.AddNewRateForProvider)]
        public async Task<IActionResult> AddNewRateForProvider([FromForm] NewRateToProviderDto model)
        {
            var check = await _clientLogicService.AddNewRateToProvider(model);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }

        /// <summary>
        /// Get all comments for provider
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<CommentsListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.AllCommentsForProvider)]
        public async Task<IActionResult> AllCommentsForProvider(string providerId)
        {
            var check = await _clientLogicService.CommentsProvider(providerId);

            return Ok(check);

        }


        /// <summary>
        /// Get My Ads for provider
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<AdsListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.GetMyAds)]
        public async Task<IActionResult> GetMyAds(int pageSize = 10, int pageNumber = 1)
        {
            var check = await _clientLogicService.MyAds(pageSize,pageNumber);

            return Ok(check);

        }


        [ProducesResponseType(typeof(BaseResponseDto<List<MyPackagesDetailsDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.GetMyPackages)]
        public async Task<IActionResult> GetMyPackages()
        {
            var data = await _clientLogicService.MyPackagesList();

            return Ok(data);

        }

        /// <summary>
        /// Cron job to check is package date is ended or not
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.CheckIsPackageIsEndedOrNot)]
        [AllowAnonymous]
        public async Task<IActionResult> CheckIsPackageIsEndedOrNot()
        {
            var check = await _clientLogicService.CheckIsPackageIsEndedOrNot();
            return Ok(new { key = 1 });
        }

        /// <summary>
        /// Resubscribe to the package
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.ResubscribeInPackage)]
        public async Task<IActionResult> ResubscribeInPackage(int userPackageId)
        {
            var check = await _clientLogicService.ReNewSubscribeInPackageByWallet(userPackageId);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }

        /// <summary>
        ///Get Ads Details To Update
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<AdsListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.ClientLogic.GetAdsDetailsToUpdate)]
        public async Task<IActionResult> GetAdsDetailsToUpdate(int adId)
        {
            var data = await _clientLogicService.AdsDetailsToUpdate(adId);

            return Ok(data);

        }

        
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.UpdateAds)]
        public async Task<IActionResult> UpdateAds([FromForm] UpdateAdsDto model)
        {

          
          
            var check = await _clientLogicService.UpdateAds(model);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }
        /// <summary>
        /// Remove Ad
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.ClientLogic.RemoveAd)]
        public async Task<IActionResult> RemoveAd(int adId)
        {
            var check = await _clientLogicService.RemoveAds(adId);
            if (check.IsSuccess)
            {
                return Ok(check);
            }
            else
            {
                return BadRequest(check);
            }
        }

    }
}
