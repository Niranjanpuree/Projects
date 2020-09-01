
namespace AutoCare.Product.Vcdb.Model
{
    public class VehicleToBedConfig:EntityBase,IDomainEntity
    {
        public int VehicleId { get; set; }
        public int BedConfigId { get; set; }
        public string Source { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual BedConfig BedConfig { get; set; }
    }
}
