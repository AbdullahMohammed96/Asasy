using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.More
{
    public class ComplaintsDetailsDto
    {

        public int complaintId { get; set; }
        public string codeComplaint { get; set; }
        public string status { get; set; }
        public string date { get; set; }
        public string message { get; set; }
        public string replay { get; set; }
        public string userName { get; set; }
        public string userImage { get; set; }

    }
}
