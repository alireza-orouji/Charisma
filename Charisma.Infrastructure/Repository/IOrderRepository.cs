using Charisma.Domain.Dtos;
using Charisma.Domain.Entities;
using Charisma.Domain.Entities.type;
using Charisma.Infrastructure.Core.Exceptions;
using Charisma.Infrastructure.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Infrastructure.Repository
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(CreateOrderDto dto, int customerId, CancellationToken cancellationToken);
        Task<bool> FinalizeOrderAsync(int orderId, CancellationToken cancellationToken);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextService dbContext;

        public OrderRepository(DbContextService dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto dto, int customerId, CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var orderModel = new Order
                {
                    CustomerId = customerId,
                    DiscountPercent = dto.DiscountPercent,
                    DiscountPrice = dto.DiscountPrice,
                    DiscountTypeId = dto.DiscountTypeId,
                    StatusId = OrderStatus.Status.New.ToInt()
                };
                var order = await dbContext.Order.AddAsync(orderModel);
                await dbContext.SaveChangesAsync(cancellationToken);

                foreach (var item in dto.Details)
                {
                    await dbContext.OrderDetails.AddAsync(new OrderDetails
                    {
                        OrderId = order.Entity.Id,
                        CommodityId = item.CommodityId,
                        Count = item.Count,
                    });
                }
                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return order.Entity.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

        }

        public async Task<bool> FinalizeOrderAsync(int orderId, CancellationToken cancellationToken)
        {
            var orderDetails = await dbContext.OrderDetails
                .Include(i => i.Order)
                .Include(i => i.Commodity)
                .Where(x => x.Order.Id == orderId)
                .ToListAsync(cancellationToken);

            if (orderDetails == null)
                throw new ApiResponseExeption(404, "No customization found for this user");

            TimeSpan start = new(8, 0, 0);
            TimeSpan end = new(17, 0, 0);
            TimeSpan now = DateTime.Now.TimeOfDay;

            if ((now < start) || (now > end))
                throw new ApiResponseExeption(405, "It is not possible to register an order outside the time limit");

            if (orderDetails.Sum(s => s.Count * s.Commodity.SellingPrice) < 500000)
                throw new ApiResponseExeption(405, "The total amount of orders must be above 50,000 Tomans");

            var order = orderDetails.FirstOrDefault().Order;

            if (orderDetails.Any(x => x.Commodity.CommodityTypeId == CommodityType.Type.Breaking.ToInt()))
                order.DeliveryStatusId = DeliveryStatus.Type.ExpressPost.ToInt();

            order.StatusId = OrderStatus.Status.Ok.ToInt();

            dbContext.Order.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
