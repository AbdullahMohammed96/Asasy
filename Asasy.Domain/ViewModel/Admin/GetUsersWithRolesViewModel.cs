using Asasy.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Admin
{
    public class GetUsersWithRolesViewModel
    {
        public List<ApplicationDbUser> users { get; set; }
        public List<string> roles { get; set; }
    }
}
