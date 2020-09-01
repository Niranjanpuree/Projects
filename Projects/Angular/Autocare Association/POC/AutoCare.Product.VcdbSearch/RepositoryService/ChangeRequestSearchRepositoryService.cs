using AutoCare.Product.Search.RepositoryService;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.VcdbSearch.RepositoryService
{
    public class ChangeRequestSearchRepositoryService : AzureSearchRepositoryService<ChangeRequestDocument>, IChangeRequestSearchRepositoryService
    {
        public ChangeRequestSearchRepositoryService(string serviceName, string apiKey, string indexName)
            :base(serviceName, apiKey, indexName)
        {
        }
    }
}
