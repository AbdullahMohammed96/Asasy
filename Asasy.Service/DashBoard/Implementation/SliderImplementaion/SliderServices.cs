using AAITHelper;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Slider;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.SliderInterfaces;
using Microsoft.EntityFrameworkCore;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.SliderImplementaion
{
    public class SliderServices : ISliderServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IHelper _uploadImage;

        public SliderServices(ApplicationDbContext context, IHelper uploadImage)
        {
            _context = context;
            _uploadImage = uploadImage;
        }


        public async Task<List<SliderViewModel>> GetAllSliders()
        {
            var Sliders = await _context.Sliders.Select(s => new SliderViewModel
            {
                Id = s.Id,
                TitleAr = s.TitleAr,
                Image = DashBordUrl.baseUrlHost + s.Image,
                IsActive = s.IsActive,
                CreationDate = s.CreationDate.ToString("dd-MM-yyyy"),
                DescriptionAr = s.DescriptionAr,
                Link=s.Link,
                DescriptionEn = s.DescriptionEn,
                TitleEn = s.TitleEn
            }).ToListAsync();
            return Sliders;
        }
        public async Task<bool> CreateSlider(CreateSliderViewModel slider)
        {
            Slider NewSlider = new Slider
            {
                TitleEn = slider.TitleEn,
                DescriptionEn= slider.DescriptionEn,
                Link = slider.Link,
                DescriptionAr = slider.DescriptionAr,
                Image =_uploadImage.Upload(slider.Image,(int)FileName.Slider),
                CreationDate=HelperDate.GetCurrentDate(),
                IsActive=true,
                TitleAr=slider.TitleAr,
            };
            _context.Sliders.Add(NewSlider);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<EditSliderViewModel> GetSliderDetails(int? Id)
        {
            return await _context.Sliders.Where(s => s.Id == Id).Select(s => new EditSliderViewModel
            {
                Id = s.Id,
                TitleAr = s.TitleAr,
                TitleEn = s.TitleEn, 
                DescriptionEn= s.DescriptionEn,
                Link = s.Link,
                Image = DashBordUrl.baseUrlHost + s.Image,
                DescriptionAr= s.DescriptionAr,
            }).FirstOrDefaultAsync();
        }

        public async Task<bool> EditSlider(EditSliderViewModel slider)
        {
            try
            {
                var current = await _context.Sliders.FirstOrDefaultAsync(a => a.Id == slider.Id);

                current.TitleAr = slider.TitleAr;
                //current.TitleEn = slider.TitleEn;
                current.DescriptionAr = slider.DescriptionAr;
                //current.DescriptionEn = slider.DescriptionEn;

                current.Link = slider.Link;
                current.Image = slider.NewImage != null ? _uploadImage.Upload(slider.NewImage, (int)FileName.Slider) : current.Image;

                await _context.SaveChangesAsync();
                return true;    
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvertismentExists(slider.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

        }

        private bool AdvertismentExists(int id)
        {
            return _context.Sliders.Any(e => e.Id == id);
        }

        public async Task<bool> ChangeState(int? id)
        {
            var Slider = await _context.Sliders.FindAsync(id);
            Slider.IsActive = !Slider.IsActive;
            await _context.SaveChangesAsync();
            return Slider.IsActive;
        }

        public async Task<bool> Delete(int? id)
        {
            var slider = _context.Sliders.Where(d=>d.Id == id).FirstOrDefault();
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
