using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Xunit;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleToBrakeConfigIndexSchemaRepositoryServiceTests
    {
        [Fact()]
        public void CreateIndexTest()
        {
            var indexSchemaRepo = new VehicleToBrakeConfigIndexRepositoryService();
            indexSchemaRepo.CreateIndex();
        }
    }
}
