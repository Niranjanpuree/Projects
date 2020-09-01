using System.Threading.Tasks;
using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.VcdbSearch.RepositoryService;

namespace AutoCare.Product.VcdbSearch.ApplicationService
{
    public class VehicleToBrakeConfigSearchService : IVehicleToBrakeConfigSearchService
    {
        private readonly IVehicleToBrakeConfigSearchRepositoryService _vehicleToBrakeConfigSearchRepositoryService;

        public VehicleToBrakeConfigSearchService(IVehicleToBrakeConfigSearchRepositoryService vehicleToBrakeConfigSearchRepositoryService)
        {
            _vehicleToBrakeConfigSearchRepositoryService = vehicleToBrakeConfigSearchRepositoryService;
        }

        public async Task<VehicleToBrakeConfigSearchResult> SearchAsync(string searchText, string filter = null, SearchOptions searchOptions = null)
        {
            var result = await _vehicleToBrakeConfigSearchRepositoryService.SearchAsync(searchText, filter, searchOptions);
            VehicleToBrakeConfigSearchResult vehicleToBrakeConfigSearchResult = new VehicleToBrakeConfigSearchResult()
            {
                Documents = result.Documents,
                Facets = result.Facets,
                TotalCount = result.TotalCount
            };

            return vehicleToBrakeConfigSearchResult;
        }
    }
}
