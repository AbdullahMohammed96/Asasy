using AAITHelper.Enums;
using Asasy.Domain.Enums;
using Asasy.Domain.Model;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.HomeInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.HomeImplementation
{
    public class HomeServices : IHomeServices
    {
        private readonly ApplicationDbContext _context;

        public HomeServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public DashBoardHomeModel HomeIndex()
        {
            var data = (from st in _context.Settings
                        let UserCount = _context.Users.Where(x => x.TypeUser == UserType.Client.ToNumber()).Count()
                        select new DashBoardHomeModel
                        {
                            UserCount = UserCount
                        }).FirstOrDefault();

            return data;
        }
    }
}
