using AAITHelper;
using Asasy.Domain.Entities.AsasyPackages;
using Asasy.Domain.ViewModel.Package;
using Asasy.Domain.ViewModel.Users;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.PackagesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.PackageImplementation
{
    public class PackageService : IPackageService
    {
        private readonly ApplicationDbContext _context;

        public PackageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNewPackage(AddNewPackageViewModel model)
        {
            try
            {


                AsasyPackage package = new AsasyPackage
                {
                    CountDayes = model.CountDayes,
                    CreationDate = HelperDate.GetCurrentDate(),
                    DescriptionAr = model.DescriptionAr,
                    DescriptionEn = model.DescriptionEn,
                    IsActive = true,
                    NameAr = model.NameAr,
                    NameEn = model.NameEn,
                    Price = model.Price
                };

                _context.Packages.Add(package);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ChangeState(int id)
        {
            var package = _context.Packages.Where(d => d.Id == id).FirstOrDefault();
            package.IsActive = !package.IsActive;
            _context.SaveChanges();

            return package.IsActive;
        }

        public async Task<CheckDataViewModel> Delete(int id)
        {
            var package = _context.Packages.Where(d => d.Id == id).FirstOrDefault();
            if (package != null)
            {
                _context.Packages.Remove(package);
                _context.SaveChanges();
                return new CheckDataViewModel
                {
                    Check = true,
                    Message = "تم الحذف بنجاح"
                };
            }
            return new CheckDataViewModel
            {
                Message = "حدث خطأ ما"
            };
        }

        public async Task<bool> EditPackage(PackageDetailsViewModel model)
        {
            var package = _context.Packages.Where(d=>d.Id == model.Id).FirstOrDefault();
            if (package != null)
            {
                package.NameAr = model.NameAr;
                package.NameEn= model.NameEn;
                package.DescriptionAr = model.DescriptionAr;
                package.DescriptionEn= model.DescriptionEn;
                package.Price= model.Price;
                package.CountDayes= model.CountDayes;

                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<PackageDetailsViewModel> PackageDetails(int PackageId)
        {
            var package = _context.Packages.Where(d=>d.Id == PackageId).Select(c=> new PackageDetailsViewModel
            {
                Id = PackageId,
                CountDayes=c.CountDayes,
                DescriptionAr=c.DescriptionAr,
                DescriptionEn=c.DescriptionEn,
                NameAr=c.NameAr,
                NameEn=c.NameEn,
                Price = c.Price,
            }).FirstOrDefault();

            return package;
        }

        public async Task<List<PackagesListViewModel>> Packages()
        {
            var packages = _context.Packages.Select(c => new PackagesListViewModel
            {
                Id = c.Id,
                CountDayes = c.CountDayes,
                CreationDate = c.CreationDate.ToString("dd/MM/yyyy"),
                DescriptionAr = c.DescriptionAr,
                DescriptionEn = c.DescriptionEn,
                IsActive = c.IsActive,
                NameAr = c.NameAr,
                NameEn = c.NameEn,
                Price = c.Price
            }).OrderByDescending(o => o.Id).ToList();

            return packages;
        }
    }
}
