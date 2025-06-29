using Asasy.Domain.Entities.Copon;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.ViewModel.Copon;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.CoponsInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.CoponsImplementation
{
    public class CoponServices : ICoponServices
    {
        private readonly ApplicationDbContext _context;

        public CoponServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CoponViewModel>> GetCopons()
        {
            var copons = await _context.Copon.OrderByDescending(x => x.Date).Select(cop => new CoponViewModel
            {
                Id = cop.Id,
                count = cop.Count,
                coponCode = cop.CoponCode,
                countUsed = cop.CountUsed,
                discount = cop.Discount,
                limtdiscount = cop.limtDiscount,
                expirdate = cop.Expirdate.ToString("dd/MM/yyyy"),
                isActive = cop.IsActive
            }).ToListAsync();

            return copons;
        }

        public async Task<bool> CreateCopon(CoponCreateViewModel createCoponViewModel)
        {
            Copon copon = new Copon()
            {
                IsActive = true,
                Date = DateTime.Now,
                Count = (int)createCoponViewModel.Count,
                Expirdate = (DateTime)createCoponViewModel.expirdate,
                CoponCode = createCoponViewModel.CoponCode,
                //CountUsed = createCoponViewModel.CountUsed,
                Discount = (double)createCoponViewModel.Discount,
                limtDiscount = (double)createCoponViewModel.limt_discount
            };

            _context.Add(copon);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Copon> GetCopon(int? CoponId)
        {
            var copon = await _context.Copon.FindAsync(CoponId);
            return copon;
        }

        public async Task<bool> EditCopon(int id, CoponCreateViewModel createCoponViewModel)
        {
            var foundedCopon = await _context.Copon.FindAsync(id);

            foundedCopon.Count = (int)createCoponViewModel.Count;
            foundedCopon.Discount = (double)createCoponViewModel.Discount;
            foundedCopon.IsActive = createCoponViewModel.IsActive;
            foundedCopon.Expirdate = (DateTime)createCoponViewModel.expirdate;
            foundedCopon.limtDiscount = (double)createCoponViewModel.limt_discount;

            _context.Update(foundedCopon);

            return await _context.SaveChangesAsync() > 0;
        }

        public bool IsExist(string CoponCode)
        {
            bool IsExist = _context.Copon.Where(x => x.CoponCode == CoponCode).Any();
            return IsExist;
        }
        public bool IsExist(int? CoponId)
        {
            bool IsExist = _context.Copon.Where(x => x.Id == CoponId).Any();
            return IsExist;
        }
        public async Task<bool> ChangeState(int? id)
        {
            var copon = _context.Copon.Find(id);
            if (copon.IsActive)
            {
                copon.IsActive = false;
            }
            else
            {
                copon.IsActive = true;
            }
            await _context.SaveChangesAsync();
            return copon.IsActive;
        }

        public async Task<bool> DeleteCopons(int? id)
        {
            var copon = await _context.Copon.FindAsync(id);
            copon.IsDeleted = true;
            await _context.SaveChangesAsync();

            return copon.IsDeleted;
        }

    }
}
