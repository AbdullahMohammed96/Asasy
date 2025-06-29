using Asasy.Domain.Entities.Cities_Tables;
using Asasy.Domain.ViewModel.Regions;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.RegionsInterfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.RegionsImplementation
{
    public class RegionServices : IRegionServices
    {
        private readonly ApplicationDbContext _context;

        public RegionServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RegionsViewModel>> GetAllRegions()
        {
            return await _context.Regions.Select(x => new RegionsViewModel
            {
                Id = x.Id,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                IsActive = x.IsActive,
                //CityName = x.City.NameAr
            }).ToListAsync();
        }

        public List<SelectListItem> GetAllCities()
        {
            return _context.Cities.Where(c=>c.IsActive).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.NameAr }).ToList();
        }

        public async Task<bool> CreateRegion(CreateRegionViewModel regionViewModel)
        {
            Region region = new Region()
            {
                Date = DateTime.Now,
                NameAr = regionViewModel.NameAr,
                NameEn = regionViewModel.NameEn,
                IsActive = true,
                //CityId = regionViewModel.CityId,
            };
            await _context.Regions.AddAsync(region);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<EditRegionViewModel> GetRegionDetails(int? Id)
        {    
            var region = await _context.Regions.Where(r=>r.Id==Id)
                                       .Select(r=>new EditRegionViewModel {
                                           Id = r.Id,
                                           NameAr = r.NameAr,   
                                           NameEn = r.NameEn,
                                           //CityId=r.CityId, 
                                       }).FirstOrDefaultAsync();

            return region;
        }

        public List<SelectListItem> GetAllCitiesWithSelectedCity(int CityId)
        {
            return _context.Cities.Where(c => c.IsActive).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.NameAr, Selected = CityId == c.Id }).ToList();
        }

        public async Task<bool> EditRegion(EditRegionViewModel editRegionViewModel)
        {
            Region region = await _context.Regions.FindAsync(editRegionViewModel.Id);

            if (region == null)
                return false;

            region.NameAr = editRegionViewModel.NameAr;
            region.NameEn = editRegionViewModel.NameEn;
            //region.CityId = editRegionViewModel.CityId;
            _context.Regions.Update(region);
            await _context.SaveChangesAsync();

            return true;
        }

        public async  Task<bool> ChangeState(int? id)
        {
            Region region = await _context.Regions.FindAsync(id);
            region.IsActive = !region.IsActive;
            await _context.SaveChangesAsync();

            return region.IsActive;
        }

        public  List<SelectListItem> GetRegions()
        {
            var data  =  _context.Regions.Where(c => c.IsActive).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.NameAr }).ToList();
            return data;

        }

        public async Task<bool> DeleteRegion(int RegionId)
        {
            var isValid = _context.AdvertsmentDetails.Where(d=>d.RegionId == RegionId && !d.IsDelete).Any();
            if(isValid)
            {
                return false;
            }
            else
            {
                var region = _context.Regions.Where(d=>d.Id == RegionId).FirstOrDefault();
                region.IsDeleted = true;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
