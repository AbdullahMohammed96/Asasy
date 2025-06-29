using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.More
{
    public class ComplaintsListDto
    {
        public string codeComplaint { get; set; }
        public int complaintId { get; set; }
        public string date { get; set; }
        public string status { get; set; }
    }
}
