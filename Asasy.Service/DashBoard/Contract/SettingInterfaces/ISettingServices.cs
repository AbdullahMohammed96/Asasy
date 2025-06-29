using Asasy.Domain.DTO.OrderDTO;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.ViewModel.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.SettingServices
{
    public interface ISettingServices
    {
        Task<SettingEditViewModel> GetSetting(int? id);
        Task<bool> EditSetting(SettingEditViewModel settingEditViewModel);
        bool SettingExists(int id);
    }
}
