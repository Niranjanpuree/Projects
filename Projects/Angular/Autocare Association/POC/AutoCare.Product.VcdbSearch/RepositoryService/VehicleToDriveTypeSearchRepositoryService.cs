using AutoCare.Product.Search.RepositoryService;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.VcdbSearch.RepositoryService
{
    public class VehicleToDriveTypeSearchRepositoryService : AzureSearchRepositoryService<VehicleToDriveTypeDocument>, IVehicleToDriveTypeSearchRepositoryService
    {
        public VehicleToDriveTypeSearchRepositoryService(string serviceName, string apiKey, string indexName) 
            : base(serviceName, apiKey, indexName)
        {
        }
    }
}
