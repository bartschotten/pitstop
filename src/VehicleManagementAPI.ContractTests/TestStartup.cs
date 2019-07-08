using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Pitstop.Application.VehicleManagement;
using Pitstop.Application.VehicleManagement.DataAccess;
using Pitstop.Infrastructure.Messaging;
using System.Reflection;

namespace VehicleManagementAPI.ContractTests
{
    public class TestStartup : BaseStartup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient<IVehicleRepository, FakeVehicleRepository>();
            services.AddTransient<IMessagePublisher>(m => new Mock<IMessagePublisher>().Object);
        }

        public void Configure(IApplicationBuilder app)
        {
            base.ConfigureApp(app);

            app.UseMvc();
        }      
    }
}
