namespace AutoCare.Product.Vcdb.Model
{
    public class VehicleToMfrBodyCode : EntityBase, IDomainEntity
    {
        public int VehicleId { get; set; }
        public int MfrBodyCodeId { get; set; }
        public string Source { get; set; }
        public long? ChangeRequestId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual MfrBodyCode MfrBodyCode { get; set; }
    }
}
