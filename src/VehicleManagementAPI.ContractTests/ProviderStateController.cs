using Microsoft.AspNetCore.Mvc;
using Pitstop.Application.VehicleManagement.Model;
using System;

namespace VehicleManagementAPI.ContractTests
{
    [Route("provider-states")]
    [ApiController]
    public class ProviderStateController : Controller
    {
        [HttpPost]
        public IActionResult Post(ProviderState providerState)
        {
            if (string.IsNullOrEmpty(providerState?.State))
            {
                return Ok();
            }

            var state = providerState.State;

            if (state.StartsWith("there is a vehicle with license number"))
            {
                var licenseNumber = state.Split("'")[1];
                var vehicle = new Vehicle
                {
                    LicenseNumber = licenseNumber,
                    Brand = "Maserati",
                    Type = "Quattroporte",
                    OwnerId = Guid.NewGuid().ToString()
                };
                FakeVehicleRepository.Vehicles.Add(vehicle);
            }

            return Ok();
        }
    }
}
