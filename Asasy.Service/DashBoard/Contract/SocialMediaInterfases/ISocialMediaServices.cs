using Asasy.Domain.ViewModel.Settings;
using Asasy.Domain.ViewModel.SocialMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.SocialMediaInterfases
{
    public interface ISocialMediaServices
    {
        Task<List<SocialMediaViewModel>> GetSocialMedia();
        Task<bool> CreateSocialMedia(SocialMediaAddViewModel model);
        Task<SocialMediaEditViewModel> GetSocialMediaDetails(int? id);
        Task<bool> EditSocialMediaDetails(SocialMediaEditViewModel model);
        Task<bool> ChangeState(int? id);

    }
}
