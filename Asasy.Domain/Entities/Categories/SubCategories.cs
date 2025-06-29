using Asasy.Domain.Entities.Advertsments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.Categories
{
    public class SubCategories : BaseEntity
    {
        public SubCategories()
        {
            Ads = new HashSet<AdvertsmentDetails>();
        }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        public virtual ICollection<AdvertsmentDetails> Ads { get; set; }

    }
}
