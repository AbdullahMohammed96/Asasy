using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Complaints
{
    public class ComplaintsListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string Phone { get; set; }
        public string Replay { get; set; }
        public bool IsReplay { get; set; }
        public string CreationDate { get; set; }
        public string CodeComplaint { get; set; }
    }
}
