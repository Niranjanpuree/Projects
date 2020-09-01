using AutoCare.Product.Web.Models.Shared;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToBodyStyleConfigSearchViewModel
    {
        public VehicleToBodyStyleConfigSearchFacets Facets { get; set; }
        public VehicleToBodyStyleConfigSearchResultViewModel Result { get; set; }
        public long? TotalCount { get; set; }
    }
}