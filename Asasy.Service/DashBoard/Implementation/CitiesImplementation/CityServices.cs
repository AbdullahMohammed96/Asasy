using Asasy.Domain.Entities.Cities_Tables;
using Asasy.Domain.ViewModel.Cities;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.CitiesInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.CitiesImplementation
{
    public class CityServices : ICityServices
    {
        private readonly ApplicationDbContext _context;

        public CityServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CitiesViewModel>> GetAllCities()
        {
            var cities = await _context.Cities.Select(x => new CitiesViewModel
            {
                Id = x.Id,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                IsActive = x.IsActive,
                Region = x.Region.NameAr
            }).ToListAsync();

            return cities;
        }

        public async Task<bool> CreateCity(CreateCityViewModel createCityViewModel)
        {
            City city = new City()
            {
                Date = DateTime.Now,
                NameAr = createCityViewModel.NameAr,
                NameEn = createCityViewModel.NameEn,
                RegionId = createCityViewModel.RegionId,
                IsActive = true,
            };
            await _context.Cities.AddAsync(city);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<EditCityViewModel> GetCityDetails(int? Id)
        {
            return await _context.Cities.Where(c => c.Id == Id)
                                            .Select(c => new EditCityViewModel
                                            {
                                                Id = c.Id,
                                                NameAr = c.NameAr,
                                                NameEn = c.NameEn,
                                                RegionId = c.RegionId,
                                            }).FirstOrDefaultAsync();
        }

        public async Task<bool> EditCity(EditCityViewModel editCityViewModel)
        {
            City city = await _context.Cities.FindAsync(editCityViewModel.Id);
            if (city == null)
                return false;

            city.NameAr = editCityViewModel.NameAr;
            city.NameEn = editCityViewModel.NameEn;
            city.RegionId = editCityViewModel.RegionId;
            _context.Cities.Update(city);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeState(int? id)
        {
            City city = await _context.Cities.FindAsync(id);
            city.IsActive = !city.IsActive;
            await _context.SaveChangesAsync();

            return city.IsActive;
        }

        public async Task<bool> DeleteCity(int CityId)
        {

            var isValid = _context.AdvertsmentDetails.Where(d => d.CityId == CityId && !d.IsDelete).Any();
            if (isValid)
            {
                return false;
            }
            else
            {
                var city = _context.Cities.Where(d => d.Id == CityId).FirstOrDefault();
                city.IsDeleted = true;
                _context.SaveChanges();
                return true;
            }
        }
    }
}
