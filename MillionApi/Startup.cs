using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Application.Common;
using Application.Dto;
using Application.Interface;
using Application.Services;
using Domain.Entities;
using Infraestructure.Interface;
using Infraestructure.MongoDb;
using Infraestructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace MillionApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
            .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(o => o.AddDefaultPolicy(p => p
                .AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
            var p = Configuration.GetSection("MongoDB");
            services.Configure<MongoSettings>(Configuration.GetSection("MongoDB"));
            services.AddSingleton<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(Application.Common.MappingProfile).Assembly);
            });

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors();
            }
            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
