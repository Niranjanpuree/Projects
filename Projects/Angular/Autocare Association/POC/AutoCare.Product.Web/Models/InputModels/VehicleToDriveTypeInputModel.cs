namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleToDriveTypeInputModel:ReferenceDataInputModel
    {
        public VehicleInputModel Vehicle { get; set; }
        public DriveTypeInputModel DriveType { get; set; }
        public int VehicleId { get; set; }
        public int DriveTypeId { get; set; }
    }
}