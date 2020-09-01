using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class SpringType: EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int FrontSpringConfigCount { get; set; }
        public int RearSpringConfigCount { get; set; }
        public int VehicleToSpringConfigCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<SpringConfig> SpringConfigs_FrontSpringTypeId { get; set; }
        public ICollection<SpringConfig> SpringConfigs_RearSpringTypeId { get; set; }
    }
}
