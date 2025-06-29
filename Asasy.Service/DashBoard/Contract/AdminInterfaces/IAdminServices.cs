using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.ViewModel.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.AdminInterfaces
{
    public interface IAdminServices
    {
        Task<bool> EditUsersInRoles(UserRolesViewModel obj);
        Task<GetUsersWithRolesViewModel> GetUsersWithRoles();
        Task<List<ApplicationDbUser>> ListUsers();
        Task<List<RolesViewModel>> ListRoles();
        Task<UserWithRolesViewModel> EditUserRoles(UserIdViewModel userId);
    }
}
