using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Rates
{
    public class RatesProvidersViewModel
    {
        public int Id { get; set; }
        public double Rate { get; set; }
        public string UserName { get; set; }
        public string ProviderName { get; set; }
        public string Comment { get; set; }
        public string CreationDate { get; set; }
    }
}
