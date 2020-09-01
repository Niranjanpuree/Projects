using AutoCare.Product.Vcdb.Model;
using System.Collections.Generic;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class BedConfigChangeRequestStagingModel : ChangeRequestStagingModel<BedConfig>
    {
        public IList<VehicleToBedConfig> ReplacementVehicleToBedConfigs { get; set; }
    }
}
