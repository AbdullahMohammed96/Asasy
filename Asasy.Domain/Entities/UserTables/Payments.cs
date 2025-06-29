using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.UserTables
{
    public class Payments
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public double Paid { get; set; }
        public int TypePay { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationDbUser User { get; set; }

    }
}
