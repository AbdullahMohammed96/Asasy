using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.SocialMedia
{
    public class SocialMediaViewModel
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string URL { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
    }
}
