using Charisma.Domain.Entities;
using Charisma.Domain.Entities.type;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Infrastructure
{
    public class DbContextService : DbContext
    {
        public DbContextService(DbContextOptions<DbContextService> options) : base(options) { }

        #region dbo
        public DbSet<Commodity> Commodity { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        #endregion

        #region type
        public DbSet<CommodityType> CommodityType { get; set; }
        public DbSet<DeliveryStatus> DeliveryStatus { get; set; }
        public DbSet<DiscountType> DiscountType { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        #endregion

    }

}
