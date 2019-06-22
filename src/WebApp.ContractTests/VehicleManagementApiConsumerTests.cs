using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using PactNet.Models;
using System.Collections.Generic;
using WebApp.RESTClients;
using Match = PactNet.Matchers.Match;
using System.Threading.Tasks;
using System.IO;

namespace WebApp.ContractTests
{
    [TestClass]
    public class VehicleManagementApiConsumerTests
    {
        private static PactConfig _pactConfig;
        private static IPactBuilder _pactBuilder;
        private readonly IMockProviderService _mockProviderService;
        private readonly int _mockServerPort = 9222;

        public VehicleManagementApiConsumerTests()
        {
            _pactConfig = new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = "..\\..\\..\\pacts",
                LogDir = "..\\..\\..\\logs"
            };

            _pactBuilder = new PactBuilder(_pactConfig)
                .ServiceConsumer("pitstop-web-app")
                .HasPactWith("vehicle-management");

            _mockProviderService = _pactBuilder.MockService(_mockServerPort, false, IPAddress.Any);
        }

        [ClassCleanup]
        public static void WriteContract()
        {
            _pactBuilder.Build();

            var publisher = new PactPublisher("http://localhost:9292");

            foreach (var file in Directory.GetFiles(_pactConfig.PactDir))
            {
                publisher.PublishToBroker(file, "1.0", new List<string> { "local" });
            }
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
                        ownerId = Match.Regex("29ab5095-50c6-43b2-aa1d-0ff13eaf1ec8", "(^([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})$)")
                    }
                });

            var consumer = new VehicleManagementAPI($"localhost:{_mockServerPort}");

            //Act + Assert
            var response = await consumer.GetVehicleByLicenseNumber(licenseNumber);

            Assert.IsNotNull(response);
            Assert.AreEqual(licenseNumber, response.LicenseNumber);
            Assert.IsNotNull(response.Brand);
            Assert.IsNotNull(response.Type);
            Assert.IsNotNull(response.OwnerId);

            _mockProviderService.VerifyInteractions();
        }
    }
}
