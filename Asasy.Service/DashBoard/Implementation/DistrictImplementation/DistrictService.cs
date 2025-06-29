using AAITHelper;
using Asasy.Domain.Entities.Cities_Tables;
using Asasy.Domain.ViewModel.Districts;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.DistrictInterface;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.DistrictImplementation
{
    public class DistrictService : IDistrictService
    {
        private readonly ApplicationDbContext _context;

        public DistrictService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateDistrict(CreateDistrictViewModel model)
        {
            try
            {


                District district = new District
                {
                    CityId = model.CityId,
                    IsActive = true,
                    Date = HelperDate.GetCurrentDate(),
                    NameAr = model.NameAr,
                    NameEn = model.NameEn
                };

                _context.Districts.Add(district);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<DistrictViewModel>> Districts()
        {
            var districts = _context.Districts.Select(c => new DistrictViewModel
            {
                Id = c.Id,
                City = c.City.NameAr,
                Region = c.City.Region.NameAr,
                NameAr = c.NameAr,
                NameEn = c.NameEn,
                IsActive = c.IsActive
            }).OrderByDescending(c => c.Id).ToList();

            return districts;
        }

        public async Task<bool> EditDistrict(EditDistrictViewModel model)
        {
            var district = _context.Districts.Where(d => d.Id == model.Id).FirstOrDefault();

            if (district != null)
            {
                district.NameAr = model.NameAr;
                district.NameEn = model.NameEn;
                district.CityId = model.CityId;

                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<EditDistrictViewModel> GetDistrictDetails(int districtId)
        {
            var district = _context.Districts.Where(d => d.Id == districtId).Select(c => new EditDistrictViewModel
            {
                CityId = c.CityId,
                Id = c.Id,
                NameAr = c.NameAr,
                NameEn = c.NameEn,
            }).FirstOrDefault();

            return district;
        }


        public async Task<bool> ChangeState(int id)
        {
            var district = await _context.Districts.FindAsync(id);
            district.IsActive = !district.IsActive;
            await _context.SaveChangesAsync();

            return district.IsActive;
        }

        public async Task<List<SelectListItem>> GetCities()
        {
            var data = _context.Cities.Where(c => c.IsActive).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.NameAr }).ToList();
            return data;

        }

        public async Task<bool> DeleteDistrict(int DistrictId)
        {
            var isValid = _context.AdvertsmentDetails.Where(d => d.DistrictId == DistrictId && !d.IsDelete).Any();
            if (isValid)
            {
                return false;
            }
            else
            {
                var region = _context.Districts.Where(d => d.Id == DistrictId).FirstOrDefault();
                region.IsDeleted = true;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
