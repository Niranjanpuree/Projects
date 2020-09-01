using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToDriveTypeSearchResultViewModel
    {
        public int DriveTypeId { get; set; }
        public int VehicleId { get; set; }
        public List<DriveTypeViewModel> DriveTypes { get; set; }
        public List<VehicleToDriveTypeViewModel> VehicleToDriveTypes { get; set; }
    }
}