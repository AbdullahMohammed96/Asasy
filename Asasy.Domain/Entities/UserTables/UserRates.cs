using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.UserTables
{
    public class UserRates
    {
        public int Id { get; set; }
        public double Rate { get; set; }
        public string UserId { get; set; }
        public string ProviderId { get; set; }
        public string Comment { get; set; }
        public DateTime CreationDate { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationDbUser User { get; set; }
        [ForeignKey(nameof(ProviderId))]
        public virtual ApplicationDbUser Provider { get; set; }

    }
}
