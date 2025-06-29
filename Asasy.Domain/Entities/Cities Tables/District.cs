using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.Cities_Tables
{
    public class District : BaseEntity
    {
        public int CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }
    }
}
