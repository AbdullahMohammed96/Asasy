using Asasy.Domain.Entities.UserTables;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Admin
{
    public class UserWithRolesViewModel
    {
        public List<IdentityRole> userRoles { get; set; }
        public ApplicationDbUser user { get; set; }

    }
}
