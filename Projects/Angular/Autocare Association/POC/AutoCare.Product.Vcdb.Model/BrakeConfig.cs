using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class BrakeConfig: EntityBase, IDomainEntity
    {
        public int FrontBrakeTypeId { get; set; }
        public int RearBrakeTypeId { get; set; }
        public int BrakeSystemId { get; set; }
        public int BrakeABSId { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<VehicleToBrakeConfig> VehicleToBrakeConfigs { get; set; }
        public virtual BrakeABS BrakeABS { get; set; }
        public virtual BrakeSystem BrakeSystem { get; set; }
        public virtual BrakeType FrontBrakeType { get; set; }
        public virtual BrakeType RearBrakeType { get; set; }
    }
}
