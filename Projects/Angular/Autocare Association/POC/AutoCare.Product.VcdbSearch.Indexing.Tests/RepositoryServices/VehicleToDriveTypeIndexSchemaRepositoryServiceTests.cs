using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Xunit;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleToDriveTypeIndexSchemaRepositoryServiceTests
    {
        [Fact()]
        public void CreateIndexTest()
        {
            var driveTypeIndexSchemaRepo = new VehicleToDriveTypeIndexRepositoryService();
            driveTypeIndexSchemaRepo.CreateIndex();
        }
    }
}
