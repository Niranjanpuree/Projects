using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class BrakeType: EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int FrontBrakeConfigCount { get; set; }
        public int RearBrakeConfigCount { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<BrakeConfig> BrakeConfigs_FrontBrakeTypeId { get; set; }
        public ICollection<BrakeConfig> BrakeConfigs_RearBrakeTypeId { get; set; }
    }
}
