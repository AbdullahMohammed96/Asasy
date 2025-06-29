using AAITHelper;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.ViewModel.Districts;
using Asasy.Domain.ViewModel.ProhibitedGoods;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.ProhibitedGoodsInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.ProhibitedGoodsImplementation
{
    public class ProhibitedGoodsService : IProhibitedGoodsService
    {
        private readonly ApplicationDbContext _context;

        public ProhibitedGoodsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ChangeStatus(int id)
        {
            var prohibitedGood = _context.ProhibitedGoods.Where(d => d.Id == id).FirstOrDefault();
            prohibitedGood.IsActive = !prohibitedGood.IsActive;
            _context.SaveChanges();
            return prohibitedGood.IsActive;
        }

        public async Task<bool> CreateProhibitedGood(NewProhibitedGoodViewModel model)
        {
            try
            {
                ProhibitedGoods data = new ProhibitedGoods
                {
                    CreationDate = HelperDate.GetCurrentDate(),
                    DescriptionAr = model.DescriptionAr,
                    DescriptionEn = model.DescriptionEn,
                    IsActive = true,
                    IsDelete = false,
                    NameAr = model.NameAr,
                    NameEn = model.NameEn
                };

                _context.ProhibitedGoods.Add(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteProhibitedGoods(int id)
        {
            var prohibitedGood = _context.ProhibitedGoods.Where(d => d.Id == id).FirstOrDefault();
            _context.ProhibitedGoods.Remove(prohibitedGood);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> EditProhibitedGood(EditProhibitedGoodsViewModel model)
        {
            try
            {


                var prohibited = _context.ProhibitedGoods.Where(d => d.Id == model.Id).FirstOrDefault();
                prohibited.NameAr = model.NameAr;
                prohibited.NameEn = model.NameEn;
                prohibited.DescriptionAr = model.DescriptionAr;
                prohibited.DescriptionEn = model.DescriptionEn;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<List<ProhibitedGoodsViewModel>> ProhibitedGoods()
        {
            var data = _context.ProhibitedGoods.Select(c => new ProhibitedGoodsViewModel
            {
                Id = c.Id,
                CreationDate = c.CreationDate.ToString("dd/MM/yyyy"),
                DescriptionAr = c.DescriptionAr,
                DescriptionEn = c.DescriptionEn,
                IsActive = c.IsActive,
                NameAr = c.NameAr,
                NameEn = c.NameEn,
            }).OrderByDescending(o => o.Id).ToList();

            return data;
        }

        public async Task<EditProhibitedGoodsViewModel> ProhibitedInfo(int id)
        {
            var data = _context.ProhibitedGoods.Where(d => d.Id == id).Select(c => new EditProhibitedGoodsViewModel
            {
                Id = c.Id,
                DescriptionAr = c.DescriptionAr,
                DescriptionEn = c.DescriptionEn,
                NameAr = c.NameAr,
                NameEn = c.NameEn
            }).FirstOrDefault();

            return data;
        }
    }
}
