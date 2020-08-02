using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParksAPI.Data;
using ParksAPI.ParkMapper;
using ParksAPI.Repository;
using ParksAPI.Repository.IRepository;

namespace ParksAPI
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
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();

            services.AddAutoMapper(typeof(ParkMappings));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ParksAPIDocNP",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Parks API Doc NP",
                        Version = "1",
                        Description = "Udemy Parky API",
                    });

                options.SwaggerDoc("ParksAPIDocTrails",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Parks API Doc Trails",
                        Version = "1",
                        Description = "Udemy Parky API",
                    });
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "ParksAPI.xml");
                options.IncludeXmlComments(filePath);
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/ParksAPIDocNP/swagger.json", "Park API NP");
                options.SwaggerEndpoint("/swagger/ParksAPIDocTrails/swagger.json", "Park API Trails");
                options.RoutePrefix = "";
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
