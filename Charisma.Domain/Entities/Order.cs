using Charisma.Domain.Entities.type;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Domain.Entities
{
    [Table(nameof(Order), Schema = "dbo")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey(nameof(OrderStatus))]
        public int StatusId { get; set; }
        public OrderStatus Status { get; set; }
        public long DiscountPercent { get; set; }
        public long DiscountPrice { get; set; }
        [ForeignKey(nameof(DiscountType))]
        public int DiscountTypeId { get; set; }
        public DiscountType DiscountType { get; set; }
        [ForeignKey(nameof(DeliveryStatus))]
        public int DeliveryStatusId { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
    }
}
