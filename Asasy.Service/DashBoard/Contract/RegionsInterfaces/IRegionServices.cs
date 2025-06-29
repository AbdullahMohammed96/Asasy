using Asasy.Domain.ViewModel.Regions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.RegionsInterfaces
{
    public interface IRegionServices
    {
        Task<List<RegionsViewModel>> GetAllRegions();
        List<SelectListItem> GetAllCities();
        List<SelectListItem> GetRegions();
        List<SelectListItem> GetAllCitiesWithSelectedCity(int CityId);
        Task<bool> CreateRegion(CreateRegionViewModel Region);
        Task<EditRegionViewModel> GetRegionDetails(int? Id);
        Task<bool> EditRegion(EditRegionViewModel Region);
        Task<bool> ChangeState(int? id);
        Task<bool> DeleteRegion(int RegionId);

    }
}
