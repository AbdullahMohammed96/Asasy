using Asasy.Domain.ViewModel.ContactUs;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.ContactUsInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.ContactUsImplementation
{
    public class ContactUsServices : IContactUsServices
    {
        private readonly ApplicationDbContext _context;

        public ContactUsServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContactUsViewModel>> GetContactUs()
        {
            return await _context.ContactUs.Select(c=>new ContactUsViewModel {
                Id= c.Id,
                UserName= c.UserName,
                Msg= c.Msg,
                Email = c.Email,
                Date=c.Date.ToString("dd-MM-yyyy")
            }).ToListAsync();
        }

        public async Task<bool> DeleteContactUs(int? id)
        {
            var contact = await _context.ContactUs.FindAsync(id);
            contact.IsDeleted = true;
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
