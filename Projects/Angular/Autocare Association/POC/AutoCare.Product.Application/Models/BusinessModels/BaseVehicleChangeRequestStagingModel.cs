using System.Collections.Generic;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class BaseVehicleChangeRequestStagingModel : ChangeRequestStagingModel<BaseVehicle>
    {
        public IList<Vehicle> ReplacementVehicles { get; set; }
    }
}
