using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Package
{
    public class PackagesListViewModel
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public double Price { get; set; }
        public int CountDayes { get; set; }
        public bool IsActive { get; set; }
        public string CreationDate { get; set; }
    }
}
