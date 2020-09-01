using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Xunit;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class VehicleToBedConfigSchemaRepositoryServiceTests
    {
        [Fact()]
        public void CreateIndexTest()
        {
            var schemaRepo = new VehicleToBedConfigIndexRepositoryService();
            schemaRepo.CreateIndex();
        }
    }
}