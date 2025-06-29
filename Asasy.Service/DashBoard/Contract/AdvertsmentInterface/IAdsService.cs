using Asasy.Domain.Entities.Advertsments;
using Asasy.Domain.ViewModel.Ads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.AdvertsmentInterface
{
    public interface IAdsService
    {
       Task<List<AdsListViewModel>> AllAds();
       Task<List<AdsSpecialListViewModel>> RequestSpecilaAds();
        Task<bool> AcceptAdsToSpecial(int id);
        Task<bool> RefuseAdsToSpecial(int id);
        Task<bool> ChangeStatus(int AdsId);
        Task<List<string>> AdImages(int AdsId);
        Task<AdEditViewModel> AdDetails(int AdsId);
        Task<bool> EditAds(AdEditViewModel model);
        Task<bool> DeleteAd(int AdsId);
        Task<List<PlaceListViewModel>> Regions(int AdsId);
        Task<List<PlaceListViewModel>> Cities(int AdsId);
        Task<List<PlaceListViewModel>> Districts(int AdsId);
        Task<List<PlaceListViewModel>> Categories(int AdsId);
        Task<List<PlaceListViewModel>> SubCategories(int AdsId);
        
    }
}
