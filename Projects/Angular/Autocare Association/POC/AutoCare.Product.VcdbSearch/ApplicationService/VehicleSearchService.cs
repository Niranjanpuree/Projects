using System.Threading.Tasks;
using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.VcdbSearch.RepositoryService;

namespace AutoCare.Product.VcdbSearch.ApplicationService
{
    public class VehicleSearchService : IVehicleSearchService
    {
        private readonly IVehicleSearchRepositoryService _vehicleSearchRepositoryService;

        public VehicleSearchService(IVehicleSearchRepositoryService vehicleSearchRepositoryService)
        {
            _vehicleSearchRepositoryService = vehicleSearchRepositoryService;
        }

        public async Task<VehicleSearchResult> SearchAsync(string searchText, string filter = null, SearchOptions searchOptions = null)
        {
            //NOTE: auto cast from SearchResult<VehicleDocument> to VehicleSearchResult did not work
            //return await _vehicleSearchRepositoryService.SearchAsync(searchText, filter, searchOptions) as VehicleSearchResult;
            var result = await _vehicleSearchRepositoryService.SearchAsync(searchText, filter, searchOptions);
            VehicleSearchResult vehicleSearchResult = new VehicleSearchResult()
            {
                Documents = result.Documents,
                Facets = result.Facets,
                TotalCount = result.TotalCount
            };

            return vehicleSearchResult;
        }
    }
}
