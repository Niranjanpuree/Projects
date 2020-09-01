using AutoCare.Product.Web.Models.Shared;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToBrakeConfigSearchViewModel
    {
        public VehicleToBrakeConfigSearchFacets Facets { get; set; }
        public VehicleToBrakeConfigSearchResultViewModel Result { get; set; }
        public long? TotalCount { get; set; }
    }
}