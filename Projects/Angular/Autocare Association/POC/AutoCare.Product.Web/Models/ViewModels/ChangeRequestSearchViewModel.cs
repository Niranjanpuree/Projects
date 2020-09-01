using AutoCare.Product.Web.Models.Shared;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class ChangeRequestSearchViewModel
    {
        public ChangeRequestSearchFacets Facets{ get; set; }
        public ChangeRequestSearchResultViewModel Result { get; set; }
        public bool CanBulkSubmit { get; set; }
        public bool IsAdmin { get; set; }
    }
}   