using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class DriveTypeInputModel: ReferenceDataInputModel
    {
        public string  Name { get; set; }
        public List<VehicleToDriveTypeInputModel> VehicleToDriveTypes { get; set; }
        public int VehicleToDriveTypeCount { get; set; }

    }
}