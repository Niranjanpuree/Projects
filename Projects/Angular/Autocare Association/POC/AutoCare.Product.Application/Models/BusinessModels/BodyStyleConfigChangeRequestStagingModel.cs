using AutoCare.Product.Vcdb.Model;
using System.Collections.Generic;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class BodyStyleConfigChangeRequestStagingModel : ChangeRequestStagingModel<BodyStyleConfig>
    {
        public IList<VehicleToBodyStyleConfig> ReplacementVehicleToBodyStyleConfigs { get; set; }
    }
}
