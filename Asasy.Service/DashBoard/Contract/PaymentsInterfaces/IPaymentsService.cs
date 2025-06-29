using Asasy.Domain.ViewModel.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.PaymentsInterfaces
{
    public interface IPaymentsService
    {
        Task<List<PaymentsListViewModel>> Payments();
    }
}
