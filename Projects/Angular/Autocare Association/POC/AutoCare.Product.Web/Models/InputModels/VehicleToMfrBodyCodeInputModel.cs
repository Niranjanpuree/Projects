namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleToMfrBodyCodeInputModel:ReferenceDataInputModel
    {
        public VehicleInputModel Vehicle { get; set; }
        public MfrBodyCodeInputModel MfrBodyCode { get; set; }
        public int VehicleId { get; set; }
        public int MfrBodyCodeId { get; set; }
    }
}