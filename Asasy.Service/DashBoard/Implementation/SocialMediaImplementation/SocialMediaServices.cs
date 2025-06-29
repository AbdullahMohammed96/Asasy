using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.SocialMedia;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.SocialMediaInterfases;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.SocialMediaImplementation
{
    public class SocialMediaServices : ISocialMediaServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IHelper _uploadImage;
        public SocialMediaServices(ApplicationDbContext context, IHelper uploadImage)
        {
            _context = context;
            _uploadImage = uploadImage;
        }

        public async Task<List<SocialMediaViewModel>> GetSocialMedia()
        {
            var SocialMedia = await _context.socialMedias
                                            .Select(s => new SocialMediaViewModel
                                            {
                                                Id = s.Id,
                                                NameAr = s.NameAr,
                                                NameEn = s.NameEn,
                                                URL = s.Url,
                                                Image = DashBordUrl.baseUrlHost + s.Image,
                                                IsActive = s.IsActive
                                            }).ToListAsync();
            return SocialMedia;
        }
        public async Task<bool> CreateSocialMedia(SocialMediaAddViewModel model)
        {
            SocialMedia newadvertisement = new SocialMedia
            {
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                Url = model.Url,
                Image = model.Img != null ? _uploadImage.Upload(model.Img, (int)FileName.SocialMedia) : "",
                IsActive = true,
                Date = DateTime.Now,
            };
            _context.Add(newadvertisement);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<SocialMediaEditViewModel> GetSocialMediaDetails(int? id)
        {
            var socialMedia = await _context.socialMedias.FindAsync(id);

            SocialMediaEditViewModel editSocialMediaViewModel = new SocialMediaEditViewModel
            {
                Id = socialMedia.Id,
                NameAr = socialMedia.NameAr,
                NameEn = socialMedia.NameEn,
                Url = socialMedia.Url,
                CurrentImage = DashBordUrl.baseUrlHost + socialMedia.Image,
            };

            return editSocialMediaViewModel;
        }

        public async Task<bool> EditSocialMediaDetails(SocialMediaEditViewModel editSocialMediaViewModel)
        {
            try
            {
                var current = await _context.socialMedias.FirstOrDefaultAsync(a => a.Id == editSocialMediaViewModel.Id);
                current.Id = editSocialMediaViewModel.Id;
                current.NameAr = editSocialMediaViewModel.NameAr;
                current.NameEn = editSocialMediaViewModel.NameEn;
                current.Url = editSocialMediaViewModel.Url;
                current.Image = editSocialMediaViewModel.NewImage != null ? _uploadImage.Upload(editSocialMediaViewModel.NewImage, (int)FileName.SocialMedia) : current.Image;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SocialMediaExists(editSocialMediaViewModel.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;    
        }
        private bool SocialMediaExists(int id)
        {
            return _context.socialMedias.Any(e => e.Id == id);
        }

        public async Task<bool> ChangeState(int? id)
        {
            var socialMedia = await _context.socialMedias.FindAsync(id);
            socialMedia.IsActive = !socialMedia.IsActive;
            await _context.SaveChangesAsync();
            return socialMedia.IsActive;
        }
    }
}
