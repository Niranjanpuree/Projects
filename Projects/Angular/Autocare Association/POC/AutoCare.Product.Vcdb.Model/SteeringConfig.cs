using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class SteeringConfig: EntityBase, IDomainEntity
    {
        public int SteeringTypeId { get; set; }
        public int SteeringSystemId { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public int VehicleToSteeringConfigCount { get; set; }
        public virtual SteeringSystem SteeringSystem { get; set; }
        public ICollection<VehicleToSteeringConfig> VehicleToSteeringConfigs { get; set; }
        public virtual SteeringType SteeringType { get; set; }
    }
}
