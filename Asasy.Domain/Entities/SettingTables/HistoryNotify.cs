using Asasy.Domain.Entities.UserTables;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asasy.Domain.Entities.SettingTables
{
    // Table History Text
    public class HistoryNotify
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int UserCountNotify { get; set; }
        public int ProviderCountNotify { get; set; }
    }
}
