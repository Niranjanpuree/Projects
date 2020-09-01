using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using Xunit;

namespace AutoCare.Product.VcdbSearch.Indexing.Tests.RepositoryServices
{
    public class ChangeRequestIndexSchemaRepositoryServiceTests
    {
        [Fact()]
        public void CreateIndexTest()
        {
            var changeRequestIndexSchemaRepo = new ChangeRequestIndexRepositoryServices();
            changeRequestIndexSchemaRepo.CreateIndex();
        }
    }
}
