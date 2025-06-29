using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.SettingTables
{
    public class SocialMedia:BaseEntity
    {
        public string Image { get; set; }
        public string Url { get; set; }
    }
}
