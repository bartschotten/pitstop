using Microsoft.EntityFrameworkCore;
using Pitstop.Application.VehicleManagement.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pitstop.Application.VehicleManagement.DataAccess
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleManagementDBContext _dbContext;

        public VehicleRepository(VehicleManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _dbContext.Vehicles.ToListAsync();
        }

        public async Task<Vehicle> GetByLicenseNumberAsync(string licenseNumber)
        {
            var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.LicenseNumber == licenseNumber);

            return vehicle;
        }

        public async Task RegisterAsync(Vehicle vehicle)
        {
            try
            {
                _dbContext.Vehicles.Add(vehicle);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new System.Exception();
            }
        }
    }
}
