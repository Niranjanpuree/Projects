using AutoCare.Product.Web.Models.Shared;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleSearchViewModel
    {
        public VehicleSearchFacets Facets { get; set; }
        public VehicleSearchResultViewModel Result { get; set; }
        public long? TotalCount { get; set; }
    }
}