using Asasy.Domain.ViewModel.ContactUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.ContactUsInterfaces
{
    public interface IContactUsServices
    {
        Task<List<ContactUsViewModel>> GetContactUs();
        Task<bool> DeleteContactUs(int? id);
    }
}
