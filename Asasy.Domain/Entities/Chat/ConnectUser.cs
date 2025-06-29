using System;
using System.ComponentModel.DataAnnotations;

namespace Asasy.Domain.Entities.Chat
{
    public class ConnectUser
    {
        [Key]
        public int Id { get; set; }
        public string ContextId { get; set; }
        public DateTime date { get; set; } = DateTime.Now;

        public string UserId { get; set; }

        //[ForeignKey("UserId")]
        //public virtual ApplicationDbUser User { get; set; }
    }
}
