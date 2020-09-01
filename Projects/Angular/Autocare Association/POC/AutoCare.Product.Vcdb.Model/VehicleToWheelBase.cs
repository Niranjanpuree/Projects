
namespace AutoCare.Product.Vcdb.Model
{
    public class VehicleToWheelBase : EntityBase, IDomainEntity
    {
        public int VehicleId { get; set; }
        public int WheelBaseId { get; set; }
        public string Source { get; set; }
        public long? ChangeRequestId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual WheelBase WheelBase { get; set; }
    }
}
