using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ChatDTO
{
    public class GetAllMessagesDto
    {
        public AdInfoDto ad {  get; set; }
        public List<ListMessageTwoUsersDto> ListMessages { get; set; }
    }
}
