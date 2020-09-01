using System.Threading.Tasks;
using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.VcdbSearch.RepositoryService;

namespace AutoCare.Product.VcdbSearch.ApplicationService
{
    public class VehicleToMfrBodyCodeSearchService:IVehicleToMfrBodyCodeSearchService
    {
        private readonly IVehicleToMfrBodyCodeSearchRepositoryService _vehicleToMfrBodyCodeSearchRepositoryService;

        public VehicleToMfrBodyCodeSearchService(IVehicleToMfrBodyCodeSearchRepositoryService vehicleToMfrBodyCodeSearchRepositoryService)
        {
            _vehicleToMfrBodyCodeSearchRepositoryService = vehicleToMfrBodyCodeSearchRepositoryService;
        }

        public async Task<VehicleToMfrBodyCodeSearchResult> SearchAsync(string searchText, string filter = null, SearchOptions searchOptions = null)
        {
            var result = await _vehicleToMfrBodyCodeSearchRepositoryService.SearchAsync(searchText, filter, searchOptions);
            VehicleToMfrBodyCodeSearchResult vehicleToMfrBodyCodeSearchResult = new VehicleToMfrBodyCodeSearchResult()
            {
                Documents = result.Documents,
                Facets = result.Facets,
                TotalCount = result.TotalCount
            };

            return vehicleToMfrBodyCodeSearchResult;
        }
    }
}
