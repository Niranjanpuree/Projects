namespace AutoCare.Product.Vcdb.Model
{
    public class VehicleToSpringConfig : EntityBase, IDomainEntity
    {
        public int VehicleId { get; set; }
        public int SpringConfigId { get; set; }
        public string Source { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public virtual SpringConfig SpringConfig { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
