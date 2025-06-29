using Asasy.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.Advertsments
{
    public class Favorites
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AdsId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationDbUser User { get; set; }
        [ForeignKey(nameof(AdsId))]
        public virtual  AdvertsmentDetails Ads { get; set; }
    }
}
