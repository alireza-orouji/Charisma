using Charisma.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Infrastructure.Repository
{
    public interface ICommodityRepository
    {
        Task<int> CreateCommodityAsync(CreateCommodityDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateCommodityAsync(UpdateCommodityDto dto, CancellationToken cancellationToken);
        Task<bool> DeleteCommodityAsync(int commodityId, CancellationToken cancellationToken);
        Task<GetCommodityDto> GetCommodityAsync(int commodityId, CancellationToken cancellationToken);
        Task<IEnumerable<GetCommodityDto>> GetsCommodityAsync(CancellationToken cancellationToken);
    }

    public class CommodityRepository : ICommodityRepository
    {
        private readonly DbContextService dbContext;

        public CommodityRepository(DbContextService dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> CreateCommodityAsync(CreateCommodityDto dto, CancellationToken cancellationToken)
        {
            var commodity = await dbContext.Commodity.AddAsync(new Domain.Entities.Commodity
            {
                Name = dto.Name,
                ProductionAmount = dto.ProductionAmount,
                SellingPrice = dto.SellingPrice,
                CommodityTypeId = dto.CommodityTypeId,
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return commodity.Entity.Id;
        }

        public async Task<bool> DeleteCommodityAsync(int commodityId, CancellationToken cancellationToken)
        {
            var commodity = await dbContext.Commodity.Where(x => x.Id == commodityId).FirstAsync();
            if (commodity != null)
            {
                dbContext.Commodity.Remove(commodity);
                await dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<GetCommodityDto?> GetCommodityAsync(int commodityId, CancellationToken cancellationToken)
        {
            var commodity = await dbContext.Commodity.Where(x => x.Id == commodityId).FirstAsync();
            if (commodity != null)
                return new GetCommodityDto
                {
                    Name = commodity.Name,
                    ProductionAmount = commodity.ProductionAmount,
                    SellingPrice = commodity.SellingPrice,
                    CommodityTypeId = commodity.CommodityTypeId,
                };
            return null;
        }

        public async Task<IEnumerable<GetCommodityDto>> GetsCommodityAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Commodity.Select(commodity => new GetCommodityDto
            {
                Name = commodity.Name,
                ProductionAmount = commodity.ProductionAmount,
                SellingPrice = commodity.SellingPrice,
                CommodityTypeId = commodity.CommodityTypeId,
            }).ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateCommodityAsync(UpdateCommodityDto dto, CancellationToken cancellationToken)
        {
            var commodity = await dbContext.Commodity.Where(x => x.Id == dto.Id).FirstAsync();
            if (commodity != null)
            {
                commodity.Name = dto.Name;
                commodity.ProductionAmount = dto.ProductionAmount;
                commodity.SellingPrice = dto.SellingPrice;
                commodity.CommodityTypeId = dto.CommodityTypeId;
                dbContext.Commodity.Update(commodity);
                await dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }
    }
}
