namespace AutoCare.Product.Vcdb.Model
{
    public class VehicleToSteeringConfig: EntityBase, IDomainEntity
    {
        public int VehicleId { get; set; }
        public int SteeringConfigId { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public virtual SteeringConfig SteeringConfig { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
