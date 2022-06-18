using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Charisma.Application
{
    public static class SwaggerConfiguration
    {
        public static void UseCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("app.v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "App Api",
                    Description = "The api for vertion 1 | LearnBox",
                    TermsOfService = new Uri("https://DidbanAfzar.com")
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                options.AddSwaggerXml();

                options.OperationFilter<AddSwaggerHeaderParameter>();

                options.CustomSchemaIds(type => type.ToString());
            });

        }

        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/app.v1/swagger.json", "Web-V1");
            });
        }

        /// <summary>
        /// جهت افزودن فایل مستندات که در اسمیلی های متفاوت برنامه می باشد
        /// </summary>
        /// <param name="assemblies"></param>
        private static void AddSwaggerXml(this SwaggerGenOptions options)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name.ToLower().Contains("daec")).ToArray();
            var paths = new List<string>();
            foreach (var assembly in assemblies)
            {
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                    paths.Add(xmlPath);
            }

            paths.ForEach(x => options.IncludeXmlComments(x));
        }
    }

    public class AddSwaggerHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "x-api-version",
                Description = "Api Versioning",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema() { Type = "String" },
                Required = false,
                Example = new OpenApiString("1.0")
            });

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "static-key",
                Description = "Static key",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema() { Type = "String" },
                Required = false,
                AllowEmptyValue = true,
            });
        }
    }
}
