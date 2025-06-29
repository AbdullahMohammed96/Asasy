using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.Cities_Tables
{
    public class Region : BaseEntity
    {
        public Region()
        {
            Cities = new HashSet<City>();
        }
        public virtual ICollection<City> Cities { get; set; }
    }
}
