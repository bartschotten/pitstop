using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Pitstop.Application.VehicleManagement.Model;
using Pitstop.Application.VehicleManagement.Commands;
using Pitstop.Application.VehicleManagement.Events;
using Microsoft.AspNetCore.Mvc;

namespace Pitstop.Application.VehicleManagement
{
    public abstract class BaseStartup
    {
        protected IConfiguration _configuration;

        public BaseStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void ConfigureApp(IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            SetupAutoMapper();
        }

        private void SetupAutoMapper()
        {
            // setup automapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<RegisterVehicle, Vehicle>();
                cfg.CreateMap<RegisterVehicle, VehicleRegistered>()
                    .ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));
            });
        }
    }
}
