using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.Notification
{
    public class HistoryNotificationViewModel
    {
        public int id { get; set; }
        public string Text { get; set; }
        public DateTime TextDate { get; set; }
        public int UserNotifyCount { get; set; }
        public int ProviderNotifyCount { get; set; }
    }
}
