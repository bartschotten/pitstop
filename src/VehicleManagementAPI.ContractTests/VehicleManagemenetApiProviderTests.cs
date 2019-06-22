using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using System.Collections.Generic;

namespace VehicleManagementAPI.ContractTests
{
    [TestClass]
    public class VehicleManagementApiProviderTests
    {
        private static string _providerUri;
        private static IWebHost _webHost;
        private static PactVerifierConfig _config;

        [ClassInitialize]
        public static void SetUpWebHost(TestContext context)
        {
            _config = new PactVerifierConfig
            {
                Outputters = new List<IOutput> { new MsTestOutput(context) },
                Verbose = true,
                ProviderVersion = "1.0",
                PublishVerificationResults = true
            };
            _providerUri = "http://localhost:9001";

            _webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
                .UseUrls(_providerUri)
                .UseStartup<TestStartup>()
                .Build();

            _webHost.Start();
        }

        [TestMethod]
        public void ShouldSatisfyContract()
        {
            var pactVerifier = new PactVerifier(_config);
            pactVerifier
                .ProviderState($"{_providerUri}/provider-states")
                .ServiceProvider($"vehicle-management", _providerUri)
                .PactUri("http://localhost:9292/pacts/provider/vehicle-management/consumer/pitstop-web-app/latest")
                .Verify();
        }

        [ClassCleanup]
        public static void StopWebHost()
        {
            _webHost.StopAsync().GetAwaiter().GetResult();
            _webHost.Dispose();
        }
    }
}
