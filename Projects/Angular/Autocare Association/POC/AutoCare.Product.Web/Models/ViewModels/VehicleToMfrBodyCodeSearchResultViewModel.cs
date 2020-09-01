using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToMfrBodyCodeSearchResultViewModel
    {
        public int MfrBodyCodeId { get; set; }
        public int VehicleId { get; set; }
        public List<MfrBodyCodeViewModel> MfrBodyCodes { get; set; }
        public List<VehicleToMfrBodyCodeViewModel> VehicleToMfrBodyCodes { get; set; }
    }
}