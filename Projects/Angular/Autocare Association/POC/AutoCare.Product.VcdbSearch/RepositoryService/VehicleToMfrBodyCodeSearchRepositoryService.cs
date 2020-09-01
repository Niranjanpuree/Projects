using AutoCare.Product.Search.RepositoryService;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.VcdbSearch.RepositoryService
{
    public class VehicleToMfrBodyCodeSearchRepositoryService : AzureSearchRepositoryService<VehicleToMfrBodyCodeDocument>, IVehicleToMfrBodyCodeSearchRepositoryService
    {
        public VehicleToMfrBodyCodeSearchRepositoryService(string serviceName, string apiKey, string indexName) 
            : base(serviceName, apiKey, indexName)
        {
        }
    }
}
