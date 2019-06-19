using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet.Infrastructure.Outputters;

namespace VehicleManagementAPI.ContractTests
{
    public class MsTestOutput : IOutput
    {
        private readonly TestContext _output;

        public MsTestOutput(TestContext context)
        {
            _output = context;
        }

        public void WriteLine(string line)
        {
            _output.WriteLine(line);
        }
    }
}
