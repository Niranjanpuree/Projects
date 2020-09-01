using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class BedLength : EntityBase, IDomainEntity
    {
        public string Length { get; set; }
        public string BedLengthMetric { get; set; }
        public int BedConfigCount { get; set; }
        public int VehicleToBedConfigCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<BedConfig> BedConfigs { get; set; }
    }
}
