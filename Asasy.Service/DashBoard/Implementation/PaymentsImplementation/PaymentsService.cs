using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Payment;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.PaymentsInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.PaymentsImplementation
{
    public class PaymentsService : IPaymentsService
    {
        private readonly ApplicationDbContext _context;

        public PaymentsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PaymentsListViewModel>> Payments()
        {
            var payments = _context.PaymentUsers.Select(c => new PaymentsListViewModel
            {
                Id = c.Id,
                Paid = c.Paid,
                PhoneNumber=c.User.PhoneNumber + " " + c.User.PhoneCode,
                TypePaye = c.TypePay == (int)TypePay.wallet ? "المحفظه" :"الكتروني",
                UserImage = DashBordUrl.baseUrlHost + c.User.ImgProfile,
                UserName = c.User.user_Name,
            }).OrderByDescending(o => o.Id).ToList();

            return payments;
        }
    }
}
