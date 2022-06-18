using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Domain.Dtos
{
    public class UpdateCommodityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long ProductionAmount { get; set; }
        public long SellingPrice { get; set; }
        public int CommodityTypeId { get; set; }
    }
}
