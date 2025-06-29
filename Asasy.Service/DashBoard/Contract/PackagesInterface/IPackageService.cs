using Asasy.Domain.ViewModel.Package;
using Asasy.Domain.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.PackagesInterface
{
    public interface IPackageService
    {
        Task<List<PackagesListViewModel>> Packages();
        Task<CheckDataViewModel> Delete(int id);

        Task<bool> ChangeState(int id);
        Task<bool> AddNewPackage(AddNewPackageViewModel model);
        Task<PackageDetailsViewModel> PackageDetails(int PackageId);
        Task<bool> EditPackage(PackageDetailsViewModel model);
    }
}
