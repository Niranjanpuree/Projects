using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Xunit;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleIndexSchemaRepositoryServiceTests
    {
        [Fact()]
        public void CreateIndexTest()
        {
            var vehicleIndexSchemaRepo = new VehicleIndexRepositoryService();
            vehicleIndexSchemaRepo.CreateIndex();
        }
    }
}