using Charisma.Domain.Dtos;
using Charisma.Infrastructure.Core.Exceptions;
using Charisma.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Charisma.Application.Controllers
{
    [ApiExplorerSettings(GroupName = "app.v1")]
    public class CommodityController : BaseApiController.V1
    {
        private readonly ICommodityRepository commodityRepository;

        public CommodityController(ICommodityRepository commodityRepository)
        {
            this.commodityRepository = commodityRepository;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCommodityAsync(CreateCommodityDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var commodityId = await commodityRepository.CreateCommodityAsync(dto, cancellationToken);
                return Ok(ApiResponseDto.Success(new
                {
                    CommodityId = commodityId
                }, message: "Commodity created successfully"));
            }
            catch (ApiResponseExeption exp)
            {
                return Ok(ApiResponseDto.Error(exp.StatusCode, exp.Message));
            }
            catch (Exception exp)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponseDto.Error(0, exp.Message, exp.InnerException?.Message));
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCommodityAsync(UpdateCommodityDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (await commodityRepository.UpdateCommodityAsync(dto, cancellationToken))
                    return Ok(ApiResponseDto.Success(message: "Commodity Update successfully"));
                throw new ApiResponseExeption(500, "The operation failed");
            }
            catch (ApiResponseExeption exp)
            {
                return Ok(ApiResponseDto.Error(exp.StatusCode, exp.Message));
            }
            catch (Exception exp)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponseDto.Error(0, exp.Message, exp.InnerException?.Message));
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCommodityAsync(int commodityId, CancellationToken cancellationToken)
        {
            try
            {
                if (await commodityRepository.DeleteCommodityAsync(commodityId, cancellationToken))
                    return Ok(ApiResponseDto.Success(message: "Commodity Delete successfully"));
                throw new ApiResponseExeption(500, "The operation failed");
            }
            catch (ApiResponseExeption exp)
            {
                return Ok(ApiResponseDto.Error(exp.StatusCode, exp.Message));
            }
            catch (Exception exp)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponseDto.Error(0, exp.Message, exp.InnerException?.Message));
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetCommodityAsync(int commodityId, CancellationToken cancellationToken)
        {
            try
            {
                var res = await commodityRepository.GetCommodityAsync(commodityId, cancellationToken);
                return Ok(res.Success());
            }
            catch (ApiResponseExeption exp)
            {
                return Ok(ApiResponseDto.Error(exp.StatusCode, exp.Message));
            }
            catch (Exception exp)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponseDto.Error(0, exp.Message, exp.InnerException?.Message));
            }
        }

        [HttpGet("Gets")]
        public async Task<IActionResult> GetsCommodityAsync(CancellationToken cancellationToken)
        {
            try
            {
                var res = await commodityRepository.GetsCommodityAsync(cancellationToken);
                return Ok(res.Success());
            }
            catch (ApiResponseExeption exp)
            {
                return Ok(ApiResponseDto.Error(exp.StatusCode, exp.Message));
            }
            catch (Exception exp)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponseDto.Error(0, exp.Message, exp.InnerException?.Message));
            }
        }

    }
}
