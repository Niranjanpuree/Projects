using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class BodyStyleConfig : EntityBase,IDomainEntity
    {
        public int BodyNumDoorsId  { get; set; }
        public int BodyTypeId { get; set; }
        public int VehicleToBodyStyleConfigCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public virtual BodyNumDoors BodyNumDoors { get; set; }
        public virtual BodyType BodyType { get; set; }
        public ICollection<VehicleToBodyStyleConfig> VehicleToBodyStyleConfigs { get; set; }
    }
}
