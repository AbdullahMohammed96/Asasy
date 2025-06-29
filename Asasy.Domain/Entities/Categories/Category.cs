using Asasy.Domain.Entities.Advertsments;
using NPOI.Util.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.Categories
{
    public class Category : BaseEntity
    {
        public Category()
        {
            SubCategories = new System.Collections.Generic.HashSet<SubCategories>();
            Ads = new System.Collections.Generic.HashSet<AdvertsmentDetails>();
        }
        public string Image { get; set; }
        public virtual ICollection<SubCategories> SubCategories { get; set; }
        public virtual ICollection<AdvertsmentDetails> Ads { get; set; }
    }
}
