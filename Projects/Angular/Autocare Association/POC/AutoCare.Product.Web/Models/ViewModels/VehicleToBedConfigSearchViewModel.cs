using AutoCare.Product.Web.Models.Shared;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToBedConfigSearchViewModel
    {
        public VehicleToBedConfigSearchFacets Facets { get; set; }
        //public VehicleToBrakeConfigSearchFilters Filters { get; set; }
        public VehicleToBedConfigSearchResultViewModel Result { get; set; }
        public long? TotalCount { get; set; }
    }
}   