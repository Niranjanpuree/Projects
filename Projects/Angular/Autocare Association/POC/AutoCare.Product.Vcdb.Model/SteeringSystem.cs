using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class SteeringSystem: EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int SteeringConfigCount { get; set; }
        public int VehicleToSteeringConfigCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<SteeringConfig> SteeringConfigs { get; set; }

    }
}
