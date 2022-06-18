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
    [Table(nameof(Commodity), Schema = "dbo")]
    public class Commodity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public long ProductionAmount { get; set; }
        public long SellingPrice { get; set; }
        [ForeignKey(nameof(CommodityType))]
        public int CommodityTypeId { get; set; }
        public CommodityType CommodityType { get; set; }
    }
}
