
namespace AutoCare.Product.Vcdb.Model
{
    public class VehicleToDriveType : EntityBase, IDomainEntity
    {
        public int VehicleId { get; set; }
        public int DriveTypeId { get; set; }
        public string Source { get; set; }
        public long? ChangeRequestId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual DriveType DriveType { get; set; }
    }
}
