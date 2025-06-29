using Asasy.Domain.ViewModel.ProhibitedGoods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.ProhibitedGoodsInterfaces
{
    public interface IProhibitedGoodsService
    {
        Task<List<ProhibitedGoodsViewModel>> ProhibitedGoods();
        Task<bool> CreateProhibitedGood(NewProhibitedGoodViewModel model);
        Task<EditProhibitedGoodsViewModel> ProhibitedInfo(int id);
        Task<bool> EditProhibitedGood(EditProhibitedGoodsViewModel model);
        Task<bool> ChangeStatus(int id);
        Task<bool> DeleteProhibitedGoods(int id);

    }
}
