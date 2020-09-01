using System.Threading.Tasks;
using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.VcdbSearch.RepositoryService;

namespace AutoCare.Product.VcdbSearch.ApplicationService
{
    public class VehicleToBodyStyleConfigSearchService : IVehicleToBodyStyleConfigSearchService
    {
        private readonly IVehicleToBodyStyleConfigSearchRepositoryService _vehicleToBodyStyleConfigSearchRepositoryService;

        public VehicleToBodyStyleConfigSearchService(IVehicleToBodyStyleConfigSearchRepositoryService vehicleToBodyStyleConfigSearchRepositoryService)
        {
            _vehicleToBodyStyleConfigSearchRepositoryService = vehicleToBodyStyleConfigSearchRepositoryService;
        }

        public async Task<VehicleToBodyStyleConfigSearchResult> SearchAsync(string searchText, string filter = null, SearchOptions searchOptions = null)
        {
            var result = await _vehicleToBodyStyleConfigSearchRepositoryService.SearchAsync(searchText, filter, searchOptions);
            VehicleToBodyStyleConfigSearchResult vehicleToBodyStyleConfigSearchResult = new VehicleToBodyStyleConfigSearchResult()
            {
                Documents = result.Documents,
                Facets = result.Facets,
                TotalCount = result.TotalCount
            };

            return vehicleToBodyStyleConfigSearchResult;
        }
    }
}
