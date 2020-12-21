using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MMB.Mangalam.Web.Service;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MMB.Mangalam.Web.Model.Helpers;
using Microsoft.IdentityModel.Tokens;
using MMB.Mangalam.Web.Model.ViewModel;
using System.IO;

namespace MMB.Mangalam.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors();
            services.AddControllers();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            var AazureStorageConnectionStringSection = Configuration.GetSection("AazureStorageConnectionString");
            services.Configure<AazureStorageConnectionString>(AazureStorageConnectionStringSection);
            services.Configure<AppSettings>(appSettingsSection);

            services.AddScoped<UserService, UserService>();
            services.AddScoped<RegistrationService, RegistrationService>();
            services.AddScoped<SecurityService, SecurityService>();          
            services.AddScoped<FileUploadService, FileUploadService>();
            services.AddScoped<SearchProfileService, SearchProfileService>();
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //jwt

            

            string connectionString = Configuration.GetSection("DefaultConnection:ConnectionString").Value;
            services.AddSingleton<ConnectionStringService>(t => new ConnectionStringService(connectionString));
            //string azureconnectionString = AazureStorageConnectionStringSection.Get<AazureStorageConnectionString>().AzureConnectionString;
            //services.AddSingleton<FileUploadStringService>(t => new FileUploadStringService(azureconnectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options => options.WithOrigins("http://localhost:4200", "http://localhost").AllowAnyMethod());

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "//index.html";
                    await next();
                }
            });
            app.UseDefaultFiles();
            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

             app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            


        }
    }
}
