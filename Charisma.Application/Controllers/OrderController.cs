using Charisma.Domain.Dtos;
using Charisma.Infrastructure.Core.Exceptions;
using Charisma.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Charisma.Application.Controllers
{
    [ApiExplorerSettings(GroupName = "app.v1")]
    public class OrderController : BaseApiController.V1
    {
        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var trackingCode = await orderRepository.CreateOrderAsync(dto, UserId, cancellationToken);
                return Ok(ApiResponseDto.Success(new
                {
                    TrackingCode = trackingCode
                }, message: "Order created successfully"));
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

        [HttpPost("{orderId}/Finalize")]
        public async Task<IActionResult> FinalizeOrderAsync(int orderId, CancellationToken cancellationToken)
        {
            try
            {
                if (await orderRepository.FinalizeOrderAsync(orderId, cancellationToken))
                    return Ok(ApiResponseDto.Success(message: "Order successfully registered"));
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
    }
}
