using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Domain.Entities.type
{
    [Table(nameof(OrderStatus), Schema = "type")]
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public enum Status
        {
            New,
            Ok,
            Cancel
        }
    }
}
