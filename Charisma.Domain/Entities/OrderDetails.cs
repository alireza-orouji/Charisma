using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Domain.Entities
{
    [Table(nameof(OrderDetails), Schema = "dbo")]
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [ForeignKey(nameof(Commodity))]
        public int CommodityId { get; set; }
        public Commodity Commodity { get; set; }
        public int Count { get; set; }

    }
}
