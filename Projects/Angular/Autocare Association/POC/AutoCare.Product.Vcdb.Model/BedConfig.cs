using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class BedConfig:EntityBase,IDomainEntity
    {
        public int BedLengthId  { get; set; }
        public int BedTypeId { get; set; }
        public int VehicleToBedConfigCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public virtual BedLength BedLength { get; set; }
        public virtual BedType BedType { get; set; }
        public ICollection<VehicleToBedConfig> VehicleToBedConfigs { get; set; }
    }
}
