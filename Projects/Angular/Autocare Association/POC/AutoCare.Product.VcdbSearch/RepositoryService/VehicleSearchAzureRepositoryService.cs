using AutoCare.Product.Search.RepositoryService;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.VcdbSearch.RepositoryService
{
    public class VehicleSearchAzureRepositoryService : AzureSearchRepositoryService<VehicleDocument>, IVehicleSearchRepositoryService
    {
        public VehicleSearchAzureRepositoryService(string serviceName, string apiKey, string indexName) 
            : base(serviceName, apiKey, indexName)
        {
        }
    }
}
