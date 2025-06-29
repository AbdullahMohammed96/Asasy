using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.SettingTables
{
    public class ProhibitedGoods
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
