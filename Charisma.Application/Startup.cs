using Charisma.Application.GlobalConfiguration;
using Charisma.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

namespace Charisma.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            ///استفاده از سرویس احراز هویت شخصی سازی شده و توکن های jwt
            services.UseCustomAuthentication(Configuration);

            /// اینجکت پارامتر های داینامیک در پروژه
           // services.Configure<DynamicConfiguration>(configuration.GetSection(nameof(DynamicConfiguration)));

            ///اینجکت کردن سرویس ها
            services.AddServiceScopes();

            /// اینجکت سرویس کشینگ
            services.AddMemoryCache();

            services.AddHttpContextAccessor();

            ///api versioning
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
                x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });

            ///کانفیگ کردن سوآگر
            services.UseCustomSwagger(Configuration);

            ///کانفیگ EFCore
            services.AddDbContext<DbContextService>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseCustomAuthentication();
            app.UseCustomSwagger();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
