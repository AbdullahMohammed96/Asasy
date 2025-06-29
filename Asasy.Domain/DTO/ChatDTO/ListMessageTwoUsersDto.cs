using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ChatDTO
{
    public class ListMessageTwoUsersDto
    {

        public int Id { get; set; }
        public int ChatId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string SenderImage { get; set; }
        public string ReceiverImage { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public int Type { get; set; }
        public int Duration { get; set; }
    }
}
