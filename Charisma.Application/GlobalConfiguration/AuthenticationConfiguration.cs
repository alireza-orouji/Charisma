using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Charisma.Application
{
    public static class AuthenticationConfiguration
    {
        /// <summary>
        /// سرویس شخصی سازی شده احراز هویت در Api ها
        /// </summary>
        /// <param name="services"></param>
        public static void UseCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(async jwt =>
                {
                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero, // default: 5 min
                        RequireSignedTokens = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("!@#Charisma#@!")),

                        RequireExpirationTime = true,
                        ValidateLifetime = true,

                        ValidateAudience = true, //default : false
                        ValidAudience = "Charisma_Audience",

                        ValidateIssuer = true, //default : false
                        ValidIssuer = "Charisma_Issuer",



                        AuthenticationType = JwtBearerDefaults.AuthenticationScheme
                    };

                    jwt.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async (context) =>
                        {
                            
                        }
                    };
                });
        }

        /// <summary>
        /// سرویس شخصی سازی شده احراز هویت در Api ها
        /// </summary>
        /// <param name="app"></param>
        public static void UseCustomAuthentication(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var principal = new ClaimsPrincipal();

                var JwtAuthentication = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

                if (JwtAuthentication?.Principal != null)
                {
                    principal.AddIdentities(JwtAuthentication.Principal.Identities);
                }

                context.User = principal;

                await next();
            });
        }
    }
}
