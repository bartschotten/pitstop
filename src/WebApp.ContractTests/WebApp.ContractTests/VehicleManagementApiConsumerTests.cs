using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using PactNet.Models;
using System.Collections.Generic;
using WebApp.RESTClients;
using Match = PactNet.Matchers.Match;
using System.Threading.Tasks;

namespace WebApp.ContractTests
{
    [TestClass]
    public class VehicleManagementApiConsumerTests
    {
        private static IPactBuilder _pactBuilder;
        private readonly IMockProviderService _mockProviderService;
        private readonly int _mockServerPort = 9222;
        private readonly string _mockProviderHostAndPort;

        public VehicleManagementApiConsumerTests()
        {
            _pactBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" })
            .ServiceConsumer("Pitstop Web App")
            .HasPactWith("Vehicle Management");

            _mockProviderService = _pactBuilder.MockService(_mockServerPort, false, IPAddress.Any);
            _mockProviderHostAndPort = $"localhost:{_mockServerPort}";
        }

        [ClassCleanup]
        public static void WriteContract()
        {
            _pactBuilder.Build();
        }

        [TestMethod]
        public async Task ShouldGetVehicleByLicenseNumber()
        {
            var licenseNumber = "HJ-476-S";

            //Arrange
            _mockProviderService.Given($"there is a vehicle with license number '{licenseNumber}'")
                .UponReceiving($"a request to return the vehicle with license number '{licenseNumber}'")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/api/vehicles/{licenseNumber}"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        brand = Match.Type("Toyota"),
                        type = Match.Type("Auris"),
                        owner = Match.Regex("29ab5095-50c6-43b2-aa1d-0ff13eaf1ec8", "(^([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})$)")
                    }
                });

            var consumer = new VehicleManagementAPI(_mockProviderHostAndPort);

            //Act //Assert
            var response = await consumer.GetVehicleByLicenseNumber(licenseNumber);

            _mockProviderService.VerifyInteractions();
        }
    }
}
