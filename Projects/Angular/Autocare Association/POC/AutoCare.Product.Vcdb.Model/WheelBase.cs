using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class WheelBase : EntityBase, IDomainEntity
    {
        public string Base { get; set; }
        public string WheelBaseMetric { get; set; }
        public int VehicleToWheelBaseCount { get; set; }
        public long? ChangeRequestId { get; set; }
        public ICollection<VehicleToWheelBase> VehicleToWheelBases { get; set; }
    }
}
