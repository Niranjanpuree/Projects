using System.Threading.Tasks;
using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.VcdbSearch.RepositoryService;

namespace AutoCare.Product.VcdbSearch.ApplicationService
{
    public class VehicleToDriveTypeSearchService : IVehicleToDriveTypeSearchService
    {
        private readonly IVehicleToDriveTypeSearchRepositoryService _vehicleToDriveTypeSearchRepositoryService;

        public VehicleToDriveTypeSearchService(IVehicleToDriveTypeSearchRepositoryService vehicleToDriveTypeSearchRepositoryService)
        {
            _vehicleToDriveTypeSearchRepositoryService = vehicleToDriveTypeSearchRepositoryService;
        }

        public async Task<VehicleToDriveTypeSearchResult> SearchAsync(string searchText, string filter = null, SearchOptions searchOptions = null)
        {
            var result = await _vehicleToDriveTypeSearchRepositoryService.SearchAsync(searchText, filter, searchOptions);
            VehicleToDriveTypeSearchResult vehicleToDriveTypeSearchResult = new VehicleToDriveTypeSearchResult()
            {
                Documents = result.Documents,
                Facets = result.Facets,
                TotalCount = result.TotalCount
            };

            return vehicleToDriveTypeSearchResult;
        }
    }
}
