using Asasy.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.SettingTables
{
    public class Complaints
    {
        public int Id { get; set; }
        public string CodeComplaint { get; set; }
        public string UserId { get; set; }
        public string? EmployeeId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string Phone { get; set; }
        public string PhoneCode { get; set; }
        public string? Replay { get; set; }
        public bool IsReplay { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreationDate { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationDbUser User { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual ApplicationDbUser Employee { get; set; }
    }
}
