using Asasy.Domain.ViewModel.Slider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.SliderInterfaces
{
    public interface ISliderServices
    {
        Task<List<SliderViewModel>> GetAllSliders();
        Task<bool> CreateSlider(CreateSliderViewModel slider);
        Task<EditSliderViewModel> GetSliderDetails(int? Id);
        Task<bool> EditSlider(EditSliderViewModel slider);
        Task<bool> ChangeState(int? id);
        Task<bool> Delete(int? id);

    }
}
