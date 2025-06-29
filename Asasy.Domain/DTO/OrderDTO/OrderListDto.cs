using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.OrderDTO
{
    public class OrderListDto
    {
        public int orderId { get; set; }
        public string orderType { get; set; }
        public string name { get; set; }
        public string img { get; set; }
        public string location { get; set; }
        public string stutes { get; set; }
        public string date { get; set; }
    }
}
