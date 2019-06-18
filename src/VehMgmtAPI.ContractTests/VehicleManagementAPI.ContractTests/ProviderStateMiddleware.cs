using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Pitstop.Application.VehicleManagement.Model;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VehicleManagementAPI.ContractTests
{
    public class ProviderStateMiddleware
    {
        private readonly RequestDelegate _next;

        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value == "/provider-states")
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                if (context.Request.Method == HttpMethod.Post.ToString() &&
                    context.Request.Body != null)
                {
                    string jsonRequestBody;
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                    {
                        jsonRequestBody = reader.ReadToEnd();
                    }

                    var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                    if (providerState != null)
                    {
                        var state = providerState.State;

                        if (state.StartsWith("a request to return the vehicle with license number"))
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
                    }

                    await context.Response.WriteAsync(string.Empty);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
