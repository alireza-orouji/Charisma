using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Domain.Entities.type
{
    [Table(nameof(DeliveryStatus), Schema = "type")]
    public class DeliveryStatus
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public enum Type
        {
            NormalPost,
            ExpressPost
        }
    }
}
