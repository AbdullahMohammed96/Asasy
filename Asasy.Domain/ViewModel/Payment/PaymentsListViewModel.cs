using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Payment
{
    public class PaymentsListViewModel
    {
        public int Id { get; set; }
        public double Paid { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserImage { get; set; }
        public string TypePaye { get; set; }

    }
}
