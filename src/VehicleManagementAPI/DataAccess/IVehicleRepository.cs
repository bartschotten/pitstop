using Pitstop.Application.VehicleManagement.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitstop.Application.VehicleManagement.DataAccess
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetAllAsync();
        Task<Vehicle> GetByLicenseNumberAsync(string licenseNumber);
        Task RegisterAsync(Vehicle vehicle);
    }
}
