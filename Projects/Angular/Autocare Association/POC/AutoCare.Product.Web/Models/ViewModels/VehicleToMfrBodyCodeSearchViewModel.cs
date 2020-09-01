using AutoCare.Product.Web.Models.Shared;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToMfrBodyCodeSearchViewModel
    {
        public VehicleToMfrBodyCodeSearchFacets Facets { get; set; }
        public VehicleToMfrBodyCodeSearchResultViewModel Result { get; set; }
        public long? TotalCount { get; set; }
    }
}