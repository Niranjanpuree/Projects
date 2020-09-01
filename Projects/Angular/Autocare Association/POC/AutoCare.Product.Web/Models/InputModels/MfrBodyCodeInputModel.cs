using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class MfrBodyCodeInputModel: ReferenceDataInputModel
    {
        public string  Name { get; set; }
        public List<VehicleToMfrBodyCodeInputModel> VehicleToMfrBodyCodes { get; set; }
        public int VehicleToMfrBodyCodeCount { get; set; }

    }
}