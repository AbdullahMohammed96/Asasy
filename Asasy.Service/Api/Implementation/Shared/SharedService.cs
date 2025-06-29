using AAITHelper;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.DTO;
using Asasy.Domain.DTO.RegionDto;
using Asasy.Domain.DTO.Shared;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Lang;
using Asasy.Service.Api.Contract.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Implementation.Shared
{
    public class SharedService : ISharedService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILangService _langService;

        public SharedService(ApplicationDbContext context, ILangService langService)
        {
            _context = context;
            _langService = langService;
        }

        public async Task<BaseResponseDto<List<CategoriesListDto>>> Categories()
        {
            var lang = _langService.Lang;
            var categories = _context.Categories.Where(d => d.IsActive && d.SubCategories.Any()).Select(c => new CategoriesListDto
            {
                id = c.Id,
                name = lang == "ar" ? c.NameAr : c.NameEn,
                image = DashBordUrl.baseUrlHost + c.Image

            }).ToList();

            return new BaseResponseDto<List<CategoriesListDto>>
            {
                IsSuccess = true,
                Data = categories,
            };
        }

        public async Task<BaseResponseDto<List<SubCategoriesListDto>>> SubCategoriesByCategory(int CategoryId)
        {
            var lang = _langService.Lang;

            var subCategories = _context.SubCategories.Where(d => d.IsActive && d.CategoryId == CategoryId).Select(c => new SubCategoriesListDto
            {
                id = c.Id,
                name = lang == "ar" ? c.NameAr : c.NameEn,
                image = DashBordUrl.baseUrlHost + c.Image
            }).ToList();

            return new BaseResponseDto<List<SubCategoriesListDto>>
            {
                Data = subCategories,
                IsSuccess = true
            };
        }
        public async Task<BaseResponseDto<List<Domain.DTO.Shared.CitiesListDto>>> CitiesByRegion(int RegionId)
        {
            var lang = _langService.Lang;

            //var cities = _context.Cities.Where(d=>d.IsActive && d.RegionId == RegionId && d.Districts.Any()).Select(c=> new CitiesListDto
            //{
            //    id=c.Id,
            //    name = lang == "ar" ? c.NameAr : c.NameEn
            //}).ToList();





            var cities = _context.Cities
                            .Where(d => d.IsActive && d.RegionId == RegionId && d.Districts.Any())
                            .Select(c => new CitiesListDto
                            {
                                id = c.Id,
                                name = lang == "ar" ? c.NameAr : c.NameEn
                            })
                            .OrderByDescending(c => c.name == (lang == "ar" ? "الرياض" : "Riyadh"))
                            .ToList();


            return new BaseResponseDto<List<CitiesListDto>>
            {
                Data = cities,
                IsSuccess = true,
            };
        }

        public async Task<BaseResponseDto<List<DistrictListDto>>> DistrictsByCity(int CityId)
        {
            var lang = _langService.Lang;

            var districts = _context.Districts.Where(d => d.CityId == CityId && d.IsActive).Select(c => new DistrictListDto
            {
                id = c.Id,
                name = lang == "ar" ? c.NameAr : c.NameEn
            }).ToList();

            return new BaseResponseDto<List<DistrictListDto>>
            {
                Data = districts,
                IsSuccess = true,
            };
        }

        public async Task<BaseResponseDto<List<Domain.DTO.RegionDto.RegionListDto>>> Regions()
        {
            var lang = _langService.Lang;

            var regions = _context.Regions.Where(d => d.IsActive && d.Cities.Any()).Select(c => new Domain.DTO.RegionDto.RegionListDto
            {
                id = c.Id,
                name = lang == "ar" ? c.NameAr : c.NameEn
            }).ToList();

            return new BaseResponseDto<List<Domain.DTO.RegionDto.RegionListDto>>
            {
                IsSuccess = true,
                Data = regions,
            };
        }

        public async Task<BaseResponseDto<List<PackagesListDto>>> Packages()
        {
            var lang = _langService.Lang;
            var packages = _context.Packages.Where(d => d.IsActive).Select(c => new PackagesListDto
            {
                id = c.Id,
                date = HelperDate.FormatDate(c.CreationDate, lang),
                description = lang == "ar" ? c.DescriptionAr : c.DescriptionEn,
                name = lang == "ar" ? c.NameAr : c.NameEn,
                price = c.Price,
                duration = c.CountDayes,
            }).OrderByDescending(o => o.id).ToList();


            return new BaseResponseDto<List<PackagesListDto>>
            {
                Data = packages,
                IsSuccess = true
            };
        }

        public async Task<BaseResponseDto<PackagesListDto>> GetPackageDeails(int PackageId)
        {
            var lang = _langService.Lang;

            var package = _context.Packages.Where(d => d.Id == PackageId).Select(c => new PackagesListDto
            {
                date = HelperDate.FormatDate(c.CreationDate, lang),
                description = lang == "ar" ? c.DescriptionAr : c.DescriptionEn,
                id = c.Id,
                name = lang == "ar" ? c.NameAr : c.NameEn,
                price = c.Price
            }).FirstOrDefault();

            return new BaseResponseDto<PackagesListDto>
            {
                Data = package,
                IsSuccess = true
            };
        }
    }
}
