using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class BodyType : EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int BodyStyleConfigCount { get; set; }
        public int VehicleToBodyStyleConfigCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<BodyStyleConfig> BodyStyleConfigs { get; set; }
    }
}
