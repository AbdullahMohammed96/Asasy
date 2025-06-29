using Asasy.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.Advertsments
{
    public class ReplaiesComment
    {
        public int Id { get; set; }
        public string Replay { get; set; }
        public int  CommentId { get; set; }
        public string UserId { get; set; }
        //public string? test { get; set; }
        public DateTime CreationDate { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationDbUser User { get; set; }
        [ForeignKey(nameof(CommentId))]
        public virtual CommentAds Comment { get; set; }
    }
}
