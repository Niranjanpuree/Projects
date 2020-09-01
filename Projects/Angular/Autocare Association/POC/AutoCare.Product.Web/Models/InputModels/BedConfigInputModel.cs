using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class BedConfigInputModel: ReferenceDataInputModel
    {
        public int BedLengthId { get; set; }
        public int BedTypeId { get; set; }
        public List<VehicleToBedConfigInputModel> VehicleToBedConfigs { get; set; }
        public int VehicleToBedConfigCount { get; set; }

    }
}