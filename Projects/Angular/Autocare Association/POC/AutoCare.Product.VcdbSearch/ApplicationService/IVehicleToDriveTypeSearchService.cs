using System.Threading.Tasks;
using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.Model;

namespace AutoCare.Product.VcdbSearch.ApplicationService
{
    public interface IVehicleToDriveTypeSearchService
    {
        Task<VehicleToDriveTypeSearchResult> SearchAsync(string searchText, string filter = null, SearchOptions searchOptions = null);
    }
}
