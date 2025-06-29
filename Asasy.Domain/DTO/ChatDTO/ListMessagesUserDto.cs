using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ChatDTO
{
    public class ListMessagesUserDto
    {
        public int OrderId { get; set; }
        public int pageNumber { get; set; } = 50;
    }
}
