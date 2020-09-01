namespace AutoCare.Product.Vcdb.Model
{
    public class VehicleToBrakeConfig: EntityBase, IDomainEntity
    {
        public int VehicleId { get; set; }
        public int BrakeConfigId { get; set; }
        public string Source { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public virtual BrakeConfig BrakeConfig { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
