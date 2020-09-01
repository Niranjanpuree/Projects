using AutoCare.Product.Vcdb.Model;
using System.Collections.Generic;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class WheelBaseChangeRequestStagingModel : ChangeRequestStagingModel<WheelBase>
    {
        public IList<VehicleToWheelBase> ReplacementVehicleToWheelBases { get; set; }
    }
}
