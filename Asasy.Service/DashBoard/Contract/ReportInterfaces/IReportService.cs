using Asasy.Domain.ViewModel.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.ReportInterfaces
{
    public interface IReportService
    {
        Task<List<ReportsProviderViewModel>> ReportsProvider();
        Task<List<ReportsAdsViewModel>> ReportsAds();
        Task<bool> DeleteReport(int id);
    }
}
