using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Xunit;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleToWheelBaseSchemaRepositoryServiceTests
    {
        [Fact()]
        public void CreateIndexTest()
        {
            var schemaRepo = new VehicleToWheelBaseIndexRepositoryService();
            schemaRepo.CreateIndex();
        }
    }
}