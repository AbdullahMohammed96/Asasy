using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.Shared
{
    public class PackagesListDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public int duration { get; set; }
    }
}
