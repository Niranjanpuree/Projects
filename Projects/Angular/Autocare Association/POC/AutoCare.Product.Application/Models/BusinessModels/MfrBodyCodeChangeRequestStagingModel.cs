using AutoCare.Product.Vcdb.Model;
using System.Collections.Generic;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class MfrBodyCodeChangeRequestStagingModel : ChangeRequestStagingModel<MfrBodyCode>
    {
        public IList<VehicleToMfrBodyCode> ReplacementVehicleToMfrBodyCodes { get; set; }
    }
}
