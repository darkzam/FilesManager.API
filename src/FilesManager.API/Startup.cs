using FilesManager.Application.Common.Interfaces;
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
            //infrastructure
            services.AddDbContext<FilesManagerContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("FilesManagerDb"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            ///////////////

            //Application
            services.AddScoped<IFileMetadataService, FileMetadataService>();
            //////////////

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
