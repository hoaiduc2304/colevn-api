
using colevn.study.business.middleware;
using DNH.Infrastructure.Utility.AutoMap;
using DNHCore;
using DNHCore.Configuration;
using DNHCore.Infrastructure;
using DNHCore.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace project_api.StartUp
{
    public class DNHStartUp
    {
        public IConfiguration Configuration { get; }
        public DNHStartUp(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers()
       .AddJsonOptions(options =>
       {
           options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
       });
            services.AddMemoryCache();
            services.ConfigureApplicationServices(Configuration);
            var typeFinder = new AppDomainTypeFinder();
            services.ConfigureAutoMapperByAssemply(typeFinder.GetAssemblies());
            services.AddCors(options =>
            {

                options.AddPolicy(MainConfig.CorsPublic,
                    builder => builder
                        .WithOrigins("All")
                        // .WithOrigins(MainConfig.Setting.Cors)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                );
            });
            // Đăng ký middleware kiểm tra message
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            AppDependencyResolver.Init(services.BuildServiceProvider());
            ServiceHelper.Service = services.BuildServiceProvider();
            services.AddRouting();


        }

        public void Configure(IApplicationBuilder application, IWebHostEnvironment env)
        {
            application.UseRouting();
           
            if (env.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            application.UseMiddleware<LogMiddleware>();
            application.ConfigureRequestPipelines();

            //DynamicRouteBuilder.UseRoutes(application);
            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            // Sử dụng DynamicRouteBuilder để cấu hình các routes runtime


        }
    }
}
