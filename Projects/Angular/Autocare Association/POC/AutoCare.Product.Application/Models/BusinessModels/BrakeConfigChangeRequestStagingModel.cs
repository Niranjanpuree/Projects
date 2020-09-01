using System.Collections.Generic;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class BrakeConfigChangeRequestStagingModel : ChangeRequestStagingModel<BrakeConfig>
    {
        public IList<VehicleToBrakeConfig> ReplacementVehicleToBrakeConfigs { get; set; }
    }
}
