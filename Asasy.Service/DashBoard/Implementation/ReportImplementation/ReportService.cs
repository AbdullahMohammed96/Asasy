using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Reports;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.ReportInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.ReportImplementation
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteReport(int id)
        {
            var report = _context.Reports.Where(d=>d.Id == id).FirstOrDefault();
            if (report != null)
            {
                _context.Reports.Remove(report);
                _context.SaveChanges();
                return true;
            }
            return false;   
        }

        public async Task<List<ReportsAdsViewModel>> ReportsAds()
        {
            var reports = _context.Reports.Where(d => d.Type == (int)ReportType.Ads).Include(d=>d.User).Include(d=>d.Ads).ThenInclude(d=>d.User).Select(c => new ReportsAdsViewModel
            {
                Id = c.Id,
                AdsTitle = c.Ads.Title,
                Comment=c.Comment,
                ProviderName=c.Ads.User.user_Name,
                UserName = c.User.user_Name

            }).OrderByDescending(o => o.Id).ToList();


            return reports;
        }

        public async Task<List<ReportsProviderViewModel>> ReportsProvider()
        {
            var reports = _context.Reports.Where(d => d.Type == (int)ReportType.Provider).Include(d => d.User).Include(d => d.Provider).Select(c => new ReportsProviderViewModel
            {
                Id = c.Id,
                Comment = c.Comment,
                ProviderName = c.Provider.user_Name,
                UserName = c.User.user_Name

            }).OrderByDescending(o => o.Id).ToList();


            return reports;
        }
    }
}
