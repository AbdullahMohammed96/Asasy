using Asasy.Domain.ViewModel.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.CitiesInterfaces
{
    public interface ICityServices
    {
        Task<List<CitiesViewModel>> GetAllCities();
        Task<bool> CreateCity(CreateCityViewModel City);
        Task<EditCityViewModel> GetCityDetails(int? Id);
        Task<bool> EditCity(EditCityViewModel City);
        Task<bool> ChangeState(int? id);
        Task<bool> DeleteCity(int CityId);


    }
}
