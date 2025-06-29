using AAITHelper;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Copon;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Implementation.Copon
{
    public class CouponService : ICouponService
    {
        private readonly ApplicationDbContext _db;

        public CouponService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CheckCouponExist(string couponCode)
        {
            Domain.Entities.Copon.Copon foundedCoupon = await _db.Copon.SingleOrDefaultAsync(c => c.CoponCode == couponCode && c.IsActive);
            return foundedCoupon != null;
        }
        public async Task<int> GetCoponId(string couponCode)
        {
            int foundedCoupon = await _db.Copon.Where(c => c.CoponCode == couponCode && c.IsActive).Select(c => c.Id).FirstOrDefaultAsync();
            return foundedCoupon;
        }

        public async Task<bool> CheckCouponMaxUse(string couponCode)
        {
            Domain.Entities.Copon.Copon foundedCoupon = await _db.Copon.SingleOrDefaultAsync(x => x.CoponCode == couponCode && x.IsActive);
            return foundedCoupon.Count <= foundedCoupon.CountUsed;
        }

        public async Task<bool> CheckCouponUsage(int couponId, string userId)
        {
            Domain.Entities.Copon.CoponUsed CoponUsedForUser = await _db.CoponUsed.FirstOrDefaultAsync(x => x.CoponId == couponId && x.UserId == userId);
            return CoponUsedForUser != null;
        }

        public async Task<bool> CheckExpiredCoupon(string couponCode)
        {
            Domain.Entities.Copon.Copon foundedCoupon = await _db.Copon.SingleOrDefaultAsync(x => x.CoponCode == couponCode && x.IsActive);
            DateTime currentdate = HelperDate.GetCurrentDate(3);
            return foundedCoupon.Date.Date < currentdate.Date;
        }

        public async Task<double> GetLastTotalForUsingCoupon(string couponCode, double total)
        {
            Domain.Entities.Copon.Copon foundedCoupon = await _db.Copon.SingleOrDefaultAsync(x => x.CoponCode == couponCode && x.IsActive);
            double value = foundedCoupon.Discount / 100 * total;
            double lastTotal;
            if (value > foundedCoupon.limtDiscount)
            {
                value = foundedCoupon.limtDiscount;
                lastTotal = total - value;
            }
            else
            {
                lastTotal = total - value;
            }

            return lastTotal;
        }
    }
}
