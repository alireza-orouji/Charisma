using Charisma.Infrastructure.Core.Extentions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Charisma.Application.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseApiController : Controller
    {
        public BaseApiController()
        {
        }

        protected string IpAddress
        {
            get
            {
                var con = Request.HttpContext.Connection;
                return con.RemoteIpAddress?.ToString() + ':' + con.RemotePort.ToString();
            }
        }

        [Route("App/[controller]")]
        [ApiVersion("1.0")]
        [ApiExplorerSettings(GroupName = "app.v1")]
        public class V1 : BaseApiController
        {
            protected int UserId
            {
                get => User.Claims.First(c => c.Type == "Identity").Value.ToInt();
            }
        }
    }
}
