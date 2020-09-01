using AutoCare.Product.Search.RepositoryService;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.VcdbSearch.RepositoryService
{
    public class VehicleToBodyStyleConfigSearchRepositoryService : AzureSearchRepositoryService<VehicleToBodyStyleConfigDocument>, IVehicleToBodyStyleConfigSearchRepositoryService
    {
        public VehicleToBodyStyleConfigSearchRepositoryService(string serviceName, string apiKey, string indexName) 
            : base(serviceName, apiKey, indexName)
        {
        }
    }
}
