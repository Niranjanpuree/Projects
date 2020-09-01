using System.Threading.Tasks;
using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.VcdbSearch.RepositoryService;

namespace AutoCare.Product.VcdbSearch.ApplicationService
{
    public class VehicleToWheelBaseSearchService : IVehicleToWheelBaseSearchService
    {
        private readonly IVehicleToWheelBaseSearchRepositoryService _vehicleToWheelBaseSearchRepositoryService;

        public VehicleToWheelBaseSearchService(IVehicleToWheelBaseSearchRepositoryService vehicleToWheelBaseSearchRepositoryService)
        {
            _vehicleToWheelBaseSearchRepositoryService = vehicleToWheelBaseSearchRepositoryService;
        }

        public async Task<VehicleToWheelBaseSearchResult> SearchAsync(string searchText, string filter = null, SearchOptions searchOptions = null)
        {
            var result = await _vehicleToWheelBaseSearchRepositoryService.SearchAsync(searchText, filter, searchOptions);
            VehicleToWheelBaseSearchResult vehicleToWheelBaseSearchResult = new VehicleToWheelBaseSearchResult()
            {
                Documents = result.Documents,
                Facets = result.Facets,
                TotalCount = result.TotalCount
            };

            return vehicleToWheelBaseSearchResult;
        }
    }
}
