//using DNH.Cache;
using DNHCore;
using DNHCore.Model;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace project_api.StartUp
{
    public class AuthStartUp : IDNHStartup
    {
        public int Order => 1;

        public bool Active => true;

        public void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
        {
            if (ID4ClientConfig.setting == null)
            {
                var config = Configuration.GetSection("ID4ClientConfig");
                ID4ClientConfig authConfig = new ID4ClientConfig();
                config.Bind(authConfig);
                ID4ClientConfig.setting = authConfig;
            }

            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.Authority = ID4ClientConfig.setting.IdentityServerUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = ID4ClientConfig.setting.ApiResourceName;
                options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
            });
            //services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
        }

        public void Configure(IApplicationBuilder application)
        {
            application.UseHttpsRedirection();
            application.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials()); // allow credentials

            //  application.UseCors(MainConfig.CorsPublic);
            application.UseAuthentication();
            application.UseAuthorization();
        }


    }
}
