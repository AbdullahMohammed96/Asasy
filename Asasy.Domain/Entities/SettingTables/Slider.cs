using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.SettingTables
{
    public class Slider
    {
        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

    }
}
