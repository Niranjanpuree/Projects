using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class BrakeSystem: EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int BrakeConfigCount { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<BrakeConfig> BrakeConfigs { get; set; }
    }
}
