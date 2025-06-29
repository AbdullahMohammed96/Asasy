using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.Cities_Tables
{
    public class City : BaseEntity
    {
        public City()
        {
            Districts = new HashSet<District>();
        }
        public int RegionId { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}
