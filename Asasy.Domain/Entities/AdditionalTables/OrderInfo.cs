using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Entities.AdditionalTables
{
    public class OrderInfo
    {


        [Key]
        public int Id { get; set; }

        public string ProductOrService { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double TaxableAmount { get; set; }
        public double TaxRate { get; set; }
        public double TaxAmount { get; set; }
        public double SubTotalWithVat { get; set; }


        public string Img { get; set; }
        public int OrderId { get; set; }




        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(Asasy.Domain.Entities.AdditionalTables.Order.OrderInfos))]
        public virtual Order Orders { get; set; }


    }
}
