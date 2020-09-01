using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class SteeringType: EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int SteeringConfigCount { get; set; }
        public int VehicleToSteeringConfigCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<SteeringConfig> SteeringConfigs_SteeringTypeId { get; set; }

    }
}
