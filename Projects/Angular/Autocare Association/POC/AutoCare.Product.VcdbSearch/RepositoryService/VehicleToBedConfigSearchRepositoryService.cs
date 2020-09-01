using AutoCare.Product.Search.RepositoryService;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.VcdbSearch.RepositoryService
{
    public class VehicleToBedConfigSearchRepositoryService : AzureSearchRepositoryService<VehicleToBedConfigDocument>, IVehicleToBedConfigSearchRepositoryService
    {
        public VehicleToBedConfigSearchRepositoryService(string serviceName, string apiKey, string indexName) 
            : base(serviceName, apiKey, indexName)
        {
        }
    }
}
