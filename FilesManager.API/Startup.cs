using FilesManager.API.Core.Services;
using FilesManager.API.Core.Services.Interfaces;
using FilesManager.DA.Contexts;
using FilesManager.DA.Repositories;
using FilesManager.DA.Repositories.Interfaces;
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

            services.AddDbContext<FilesManagerContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("FilesManagerDb"));
            });

            services.AddControllers();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileMetadataService, FileMetadataService>();
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
