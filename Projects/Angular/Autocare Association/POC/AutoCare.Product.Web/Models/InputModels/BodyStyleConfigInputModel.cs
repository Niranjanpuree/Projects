using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class BodyStyleConfigInputModel: ReferenceDataInputModel
    {
        public int BodyNumDoorsId { get; set; }
        public int BodyTypeId { get; set; }
        public List<VehicleToBodyStyleConfigInputModel> VehicleToBodyStyleConfigs { get; set; }
        public int VehicleToBodyStyleConfigCount { get; set; }

    }
}