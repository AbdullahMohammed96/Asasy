using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.Advertsments
{
    public class AdvertsmentImages
    {
        public int Id { get; set; }
        public int AdsId { get; set; }
        public string Image { get; set; }
        [ForeignKey(nameof(AdsId))]
        public virtual AdvertsmentDetails Ads { get; set; }
    }
}
