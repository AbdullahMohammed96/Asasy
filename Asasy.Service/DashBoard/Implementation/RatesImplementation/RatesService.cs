using Asasy.Domain.ViewModel.Rates;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.RatesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.RatesImplementation
{
    public class RatesService : IRatesService
    {
        private readonly ApplicationDbContext _context;

        public RatesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RatesProvidersViewModel>> RatesProvider()
        {
            var rates = _context.UserRates.Select(c=> new RatesProvidersViewModel
            {
                Id = c.Id,
                Comment = c.Comment,
                CreationDate=c.CreationDate.ToString("dd/MM/yyyy"),
                ProviderName=c.Provider.user_Name,
                Rate = c.Rate,
                UserName = c.User.user_Name
            }).OrderByDescending(o=>o.Id).ToList();

            return rates;
        }
    }
}
