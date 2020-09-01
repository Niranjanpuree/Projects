using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class BedType : EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int BedConfigCount { get; set; }
        public int VehicleToBedConfigCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<BedConfig> BedConfigs { get; set; }
    }
}
