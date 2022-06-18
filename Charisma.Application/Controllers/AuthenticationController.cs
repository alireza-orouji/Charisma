using Charisma.Domain.Dtos;
using Charisma.Infrastructure.Core.Exceptions;
using Charisma.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Charisma.Application.Controllers
{
    [ApiExplorerSettings(GroupName = "app.v1")]
    public class AuthenticationController : BaseApiController.V1
    {
        private readonly IAuthenticationRepository authenticationRepository;
        public AuthenticationController(IAuthenticationRepository authenticationRepository)
        {
            this.authenticationRepository = authenticationRepository;
        }

        [AllowAnonymous]
        [HttpPost("SingIn")]
        public async Task<IActionResult> SingInAsync(SingInDto.Request dto, CancellationToken cancellationToken)
        {
            try
            {
                var response = await authenticationRepository.SingIn(dto, cancellationToken);

                return Ok(response.Success(message: "Generate Token"));
            }
            catch (ApiResponseExeption exp)
            {
                return Ok(ApiResponseDto.Error(exp.StatusCode, exp.Message));
            }
            catch (Exception exp)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponseDto.Error(StatusCodes.Status500InternalServerError, "خطای غیر منتظره رخ داد", exp));
            }
        }

        [AllowAnonymous]
        [HttpPost("SingUp")]
        public async Task<IActionResult> SingUpAsync(SingUpDto.Request dto, CancellationToken cancellationToken)
        {
            try
            {
                var response = await authenticationRepository.SingUp(dto, cancellationToken);

                return Ok(response.Success(message: "Generate Token"));
            }
            catch (ApiResponseExeption exp)
            {
                return Ok(ApiResponseDto.Error(exp.StatusCode, exp.Message));
            }
            catch (Exception exp)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponseDto.Error(StatusCodes.Status500InternalServerError, "خطای غیر منتظره رخ داد", exp));
            }
        }

        [HttpGet("CheckAuthentication")]
        public async Task<IActionResult> CheckAuthenticationAsync(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(ApiResponseDto.Success(message: "Authentication is valid"));
            }
            catch (ApiResponseExeption exp)
            {
                return Ok(ApiResponseDto.Error(exp.StatusCode, exp.Message));
            }
            catch (Exception exp)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponseDto.Error(StatusCodes.Status500InternalServerError, "خطای غیر منتظره رخ داد", exp));
            }
        }
    }
}
