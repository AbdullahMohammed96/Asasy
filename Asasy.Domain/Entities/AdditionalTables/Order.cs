using Asasy.Domain.Entities.AdditionalTables;
using Asasy.Domain.Entities.Chat;
using Asasy.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asasy.Domain.Entities.AdditionalTables
{
    public class Order
    {

        public Order()
        {
            OrderInfos = new HashSet<OrderInfo>();
            //Messages = new HashSet<Messages>();
        }



        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public string ProviderId { get; set; }


        public DateTime DateTime { get; set; }
        public string Info { get; set; }

        public int CoponId { get; set; }
        public int TypePay { get; set; }
        public int Stutes { get; set; }


        // relation with order
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(ApplicationDbUser.Orders))]
        public virtual ApplicationDbUser User { get; set; }

        [ForeignKey(nameof(ProviderId))]
        [InverseProperty(nameof(ApplicationDbUser.OrdersP))]
        public virtual ApplicationDbUser Provider { get; set; }


        // relation with orderInfo
        [InverseProperty(nameof(OrderInfo.Orders))]
        public virtual ICollection<OrderInfo> OrderInfos { get; set; }



        //public virtual ICollection<Messages> Messages { get; set; }


    }
}
