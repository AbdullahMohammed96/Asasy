using Asasy.Domain.ViewModel.Rates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.RatesInterfaces
{
    public interface IRatesService
    {
        Task<List<RatesProvidersViewModel>> RatesProvider();
    }
}
