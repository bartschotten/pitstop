using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pitstop.Application.VehicleManagement;
using Pitstop.Application.VehicleManagement.DataAccess;

namespace VehicleManagementAPI.ContractTests
{
    public class TestStartup : BaseStartup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, VehicleManagementDBContext dbContext)
        {
            app.UseMiddleware<ProviderStateMiddleware>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IVehicleRepository, FakeVehicleRepository>();
        }
    }
}
