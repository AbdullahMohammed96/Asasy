using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Districts
{
    public class DistrictViewModel
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
    }
}
