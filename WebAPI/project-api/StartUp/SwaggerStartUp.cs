using DNHCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace project_api.StartUp
{
    public class SwaggerStartUp : IDNHStartup
    {
        public int Order => 20;

        public bool Active => true;



        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddSwaggerGen(options =>
            {
                // options.OperationFilter<SwaggerDefaultValues>();
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Generation API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                // Thêm custom attribute để hỗ trợ tải lên tệp
                options.OperationFilter<SwaggerFileUploadOperationFilter>();

            });
        }
        public void Configure(IApplicationBuilder application)
        {
            application.UseSwagger();
            application.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GenerateCode v1");

            });
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class UploadFileAttribute : Attribute { }
    public class SwaggerFileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof(UploadFileAttribute)))
            {
                operation.Parameters.Clear();
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    ["file"] = new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary"
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}
