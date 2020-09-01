using System.Threading.Tasks;
using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.Model;

namespace AutoCare.Product.VcdbSearch.ApplicationService
{
    public interface IVehicleToWheelBaseSearchService
    {
        Task<VehicleToWheelBaseSearchResult> SearchAsync(string searchText, string filter = null, SearchOptions searchOptions = null);
    }
}