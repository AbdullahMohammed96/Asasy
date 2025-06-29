using Asasy.Domain.ViewModel.Districts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.DistrictInterface
{
    public interface IDistrictService
    {
        Task<List<DistrictViewModel>> Districts();
        Task<bool> CreateDistrict(CreateDistrictViewModel district);
        Task<EditDistrictViewModel> GetDistrictDetails(int districtId);
        Task<bool> EditDistrict(EditDistrictViewModel district);
        Task<bool> ChangeState(int id);
       Task<List<SelectListItem>> GetCities();
        Task<bool> DeleteDistrict(int DistrictId);




    }
}
