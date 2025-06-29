using Asasy.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asasy.Domain.Entities.Advertsments;

namespace Asasy.Domain.Entities.Chat
{
    public class Chats
    {
        public Chats()
        {
            Messages = new HashSet<Messages>();
        }
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ProviderId { get; set; }
        public int? AdsId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationDbUser User { get; set; }
        [ForeignKey(nameof(ProviderId))]
        public virtual ApplicationDbUser Provider { get; set; }
        public virtual ICollection<Messages> Messages { get; set; }
        [ForeignKey(nameof(AdsId))]
        public virtual AdvertsmentDetails Ad { get; set; }
    }
}
