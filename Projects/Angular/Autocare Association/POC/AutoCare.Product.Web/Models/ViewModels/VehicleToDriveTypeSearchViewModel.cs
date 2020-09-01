using AutoCare.Product.Web.Models.Shared;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToDriveTypeSearchViewModel
    {
        public VehicleToDriveTypeSearchFacets Facets { get; set; }
        public VehicleToDriveTypeSearchResultViewModel Result { get; set; }
        public long? TotalCount { get; set; }
    }
}