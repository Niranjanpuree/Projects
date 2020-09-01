using AutoCare.Product.Search.RepositoryService;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.VcdbSearch.RepositoryService
{
    public class VehicleToWheelBaseSearchRepositoryService : AzureSearchRepositoryService<VehicleToWheelBaseDocument>, IVehicleToWheelBaseSearchRepositoryService
    {
        public VehicleToWheelBaseSearchRepositoryService(string serviceName, string apiKey, string indexName) 
            : base(serviceName, apiKey, indexName)
        {
        }
    }
}
