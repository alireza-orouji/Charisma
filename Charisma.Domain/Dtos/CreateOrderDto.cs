using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Domain.Dtos
{
    public class CreateOrderDto
    {
        public long DiscountPercent { get; set; }
        public long DiscountPrice { get; set; }
        public int DiscountTypeId { get; set; }
        public IEnumerable<Detail> Details { get; set; }
        public class Detail
        {
            public int CommodityId { get; set; }
            public int Count { get; set; }
        }
    }
}
