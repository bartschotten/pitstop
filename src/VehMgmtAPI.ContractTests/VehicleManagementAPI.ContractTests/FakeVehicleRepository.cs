using Pitstop.Application.VehicleManagement.DataAccess;
using Pitstop.Application.VehicleManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleManagementAPI.ContractTests
{
    public class FakeVehicleRepository : IVehicleRepository
    {
        public static List<Vehicle> Vehicles = new List<Vehicle>();

        public Task<List<Vehicle>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle> GetByLicenseNumberAsync(string licenseNumber)
        {
            return Task.FromResult(Vehicles.FirstOrDefault(v => v.LicenseNumber == licenseNumber));
        }

        public Task RegisterAsync(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }
    }
}
