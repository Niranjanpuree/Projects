using AutoCare.Product.Web.Models.Shared;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToWheelBaseSearchViewModel
    {
        public VehicleToWheelBaseSearchFacets Facets { get; set; }
        public VehicleToWheelBaseSearchResultViewModel Result { get; set; }
        public long? TotalCount { get; set; }
    }
}