using System.Threading.Tasks;
using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.VcdbSearch.RepositoryService;

namespace AutoCare.Product.VcdbSearch.ApplicationService
{
    public class VehicleToBedConfigSearchService : IVehicleToBedConfigSearchService
    {
        private readonly IVehicleToBedConfigSearchRepositoryService _vehicleToBedConfigSearchRepositoryService;

        public VehicleToBedConfigSearchService(IVehicleToBedConfigSearchRepositoryService vehicleToBedConfigSearchRepositoryService)
        {
            _vehicleToBedConfigSearchRepositoryService = vehicleToBedConfigSearchRepositoryService;
        }

        public async Task<VehicleToBedConfigSearchResult> SearchAsync(string searchText, string filter = null, SearchOptions searchOptions = null)
        {
            var result = await _vehicleToBedConfigSearchRepositoryService.SearchAsync(searchText, filter, searchOptions);
            VehicleToBedConfigSearchResult vehicleToBedConfigSearchResult = new VehicleToBedConfigSearchResult()
            {
                Documents = result.Documents,
                Facets = result.Facets,
                TotalCount = result.TotalCount
            };

            return vehicleToBedConfigSearchResult;
        }
    }
}
