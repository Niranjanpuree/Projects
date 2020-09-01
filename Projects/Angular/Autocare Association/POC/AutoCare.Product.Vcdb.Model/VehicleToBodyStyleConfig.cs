
namespace AutoCare.Product.Vcdb.Model
{
    public class VehicleToBodyStyleConfig : EntityBase,IDomainEntity
    {
        public int VehicleId { get; set; }
        public int BodyStyleConfigId { get; set; }
        public string Source { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual BodyStyleConfig BodyStyleConfig { get; set; }
    }
}
