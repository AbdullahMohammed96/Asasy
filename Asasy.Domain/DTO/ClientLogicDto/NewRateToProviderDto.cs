using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO.ClientLogicDto
{
    public class NewRateToProviderDto
    {
        public double rate { get; set; }
        public string providerId { get; set; }
        public string comment { get; set; }
    }
}
