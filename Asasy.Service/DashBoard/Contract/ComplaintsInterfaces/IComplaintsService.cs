using Asasy.Domain.ViewModel.Complaints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.ComplaintsInterfaces
{
    public interface IComplaintsService
    {
        Task<List<ComplaintsListViewModel>> Complaints();
        Task<bool> ReplayToComplaints(int id, string replay,string userName);
        Task<bool> DeleteComplaints(int id);
    }
}
