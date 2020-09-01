using AutoCare.Product.Search.RepositoryService;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.VcdbSearch.RepositoryService
{
    public class VehicleToBrakeConfigSearchRepositoryService : AzureSearchRepositoryService<VehicleToBrakeConfigDocument>, IVehicleToBrakeConfigSearchRepositoryService
    {
        public VehicleToBrakeConfigSearchRepositoryService(string serviceName, string apiKey, string indexName) 
            : base(serviceName, apiKey, indexName)
        {
        }
    }
}
