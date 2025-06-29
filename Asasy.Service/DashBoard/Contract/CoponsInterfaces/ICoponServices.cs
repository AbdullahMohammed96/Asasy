using Asasy.Domain.Entities.Copon;
using Asasy.Domain.ViewModel.Copon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.CoponsInterfaces
{
    public interface ICoponServices
    {
        Task<List<CoponViewModel>> GetCopons();
        Task<bool> CreateCopon(CoponCreateViewModel createCoponViewModel);
        Task<Copon> GetCopon(int? CoponId);
        Task<bool> EditCopon(int id, CoponCreateViewModel createCoponViewModel);
        bool IsExist(string CoponCode);
        bool IsExist(int? CoponId);
        Task<bool> ChangeState(int? id);
        Task<bool> DeleteCopons(int? id);

    }
}
