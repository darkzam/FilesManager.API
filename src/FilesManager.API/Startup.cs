using FilesManager.Application.Common.Interfaces;
using FilesManager.Application.Models;
using FilesManager.Application.Services;
using FilesManager.Infrastructure.Contexts;
using FilesManager.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FilesManager.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            //var clientSecrets = GoogleClientSecrets.FromFile("credentials.json").Secrets;

            //services
            //    .AddAuthentication(o =>
            //    {
            //        o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
            //        o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
            //        o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    })
            //    .AddCookie()
            //    .AddGoogleOpenIdConnect(options =>
            //    {
            //        options.ClientId = clientSecrets.ClientId;
            //        options.ClientSecret = clientSecrets.ClientSecret;
            //    });

            services.Configure<GoogleDriveSettings>(Configuration.GetSection("GoogleDrive"));

            //infrastructure
            services.AddDbContext<FilesManagerContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("FilesManagerDb"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //application
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IFileMetadataService, FileMetadataService>();
            services.AddTransient<IGoogleService, GoogleService>();

            services.AddControllers();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
