using AutoCare.Product.Vcdb.Model;
using System.Collections.Generic;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class DriveTypeChangeRequestStagingModel : ChangeRequestStagingModel<DriveType>
    {
        public IList<VehicleToDriveType> ReplacementVehicleToDriveTypes { get; set; }
    }
}
