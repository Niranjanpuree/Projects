using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Xunit;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleToEngineConfigIndexSchemaRepositoryServiceTests
    {
        [Fact()]
        public void CreateIndexTest()
        {
            var vehicleToEngineConfigIndexSchemaRepo = new VehicleToEngineConfigIndexRepositoryService();
            vehicleToEngineConfigIndexSchemaRepo.CreateIndex();
        }
    }
}
