using Charisma.Infrastructure.Repository;

namespace Charisma.Application.GlobalConfiguration
{
    public static class DependencyInjection
    {
        public static void AddServiceScopes(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<ICommodityRepository, CommodityRepository>();
        }
    }
}
