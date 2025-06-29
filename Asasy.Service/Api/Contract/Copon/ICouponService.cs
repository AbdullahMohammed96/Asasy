using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Contract.Copon
{
    public interface ICouponService
    {
        Task<bool> CheckExpiredCoupon(string couponCode);
        Task<bool> CheckCouponMaxUse(string couponCode);
        Task<bool> CheckCouponExist(string couponCode);
        Task<int> GetCoponId(string couponCode);
        Task<bool> CheckCouponUsage(int couponId, string userId);
        Task<double> GetLastTotalForUsingCoupon(string couponCode, double total);


    }
}
