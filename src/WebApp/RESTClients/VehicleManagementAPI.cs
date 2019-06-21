using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.Models;
using Microsoft.AspNetCore.Hosting;
using Refit;
using WebApp.Commands;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace WebApp.RESTClients
{
    public class VehicleManagementAPI : IVehicleManagementAPI
    {
        private IVehicleManagementAPI _client;

        public  VehicleManagementAPI(string apiHostAndPort)
        {
            string baseUri = $"http://{apiHostAndPort}/api";
            _client = RestService.For<IVehicleManagementAPI>(baseUri);
        }

        public async Task<List<Vehicle>> GetVehicles()
        {
            return await _client.GetVehicles();
        }
        public async Task<Vehicle> GetVehicleByLicenseNumber([AliasAs("id")] string licenseNumber)
        {
            try
            {
                var vehicle = await _client.GetVehicleByLicenseNumber(licenseNumber);
                vehicle.LicenseNumber = licenseNumber;
                return vehicle;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task RegisterVehicle(RegisterVehicle command)
        {
            await _client.RegisterVehicle(command);
        }
    }
}
