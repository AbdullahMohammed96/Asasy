using Asasy.Domain.DTO;
using Asasy.Domain.DTO.ClientLogicDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Contract.ClientLogic
{
    public interface IClientLogicService
    {
        Task<BaseResponseDto<string>> AddNewAds(AddNewAdsDto model);
        Task<BaseResponseDto<string>> SubscribeInPackageByWallet(int adsId, int packageId);
        Task<BaseResponseDto<string>> WalletCharging(double balance);
        Task<BaseResponseDto<string>> ReNewSubscribeInPackageByWallet(int userPackageId);
        Task<BaseResponseDto<List<MyPackagesDetailsDto>>> MyPackagesList();
        Task<BaseResponseDto<List<SlidersListDto>>> Sliders();
        Task<BaseResponseDto<List<AdsListDto>>> Home(int PageSize = 10, int PageNumber = 1);
        Task<BaseResponseDto<List<AdsListDto>>> AdsByLocation(string lat, string lng, int PageSize = 10, int PageNumber = 1);

        Task<BaseResponseDto<List<AdsListDto>>> FiltrationAds(FiltrationAdsDto model);
        Task<BaseResponseDto<List<AdsListDto>>> MyAds(int PageSize = 10, int PageNumber = 1);
        Task<BaseResponseDto<AdsDetailsToUpdateDto>> AdsDetailsToUpdate(int adsId);
        Task<BaseResponseDto<string>> UpdateAds(UpdateAdsDto model);
        Task<BaseResponseDto<string>> RemoveAds(int adsId);
        Task<BaseResponseDto<List<AdsListDto>>> ListOfAdsByCategoryAndSubCategoryAndTitle(int CategoryId,int SubCategoryId,string Title, int PageSize = 10, int PageNumber = 1);
        Task<BaseResponseDto<AdsDetailsDto>> AdsDetails(int adsId, int PageSize = 10, int PageNumber = 1);
        Task<BaseResponseDto<string>> AddNewCommentForAd(int adsId, string comment);
        Task<BaseResponseDto<string>> AddNewReplayForComment(int commentId, string comment);
        Task<BaseResponseDto<List<AdsCommentDto>>> CommentsAds(int adsId, int PageSize = 10, int PageNumber = 1);
        Task<BaseResponseDto<string>> AddAdsToFavorite(int adId);
        Task<BaseResponseDto<string>> RemoveAdFromFavorite(int adId);
        Task<BaseResponseDto<List<FavoriteListDto>>> FavoriteList();
        Task<BaseResponseDto<string>> ReportAds(ReportAdsDto model);
        Task<BaseResponseDto<string>> ReportProvider(ReportProviderDto model);
        Task<BaseResponseDto<string>> FollowAd(int adId);
        Task<BaseResponseDto<string>> FollowProvider(string providerId);
        Task<BaseResponseDto<string>> UnFollowAd(int adId);
        Task<BaseResponseDto<string>> UnFollowProvider(string providerId);
        Task<BaseResponseDto<List<FollowsAdsListDto>>> FollowsAds();
        Task<BaseResponseDto<List<FollowsProviderListDto>>> FollowsProvider();
        Task<BaseResponseDto<ProviderInfoDto>> ProviderInfo(string providerId, int PageSize = 10, int PageNumber = 1);
        Task<BaseResponseDto<string>> AddNewRateToProvider(NewRateToProviderDto model);
        Task<BaseResponseDto<List<CommentsListDto>>> CommentsProvider(string providerId);


        Task<BaseResponseDto<string>> CheckIsPackageIsEndedOrNot();


    }
}
