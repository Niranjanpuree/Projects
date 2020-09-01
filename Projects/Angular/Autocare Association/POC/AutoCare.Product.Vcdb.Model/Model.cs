using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class Model : EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int VehicleTypeId { get; set; }
        public virtual VehicleType VehicleType { get; set; }
        public int BaseVehicleCount { get; set; }
        public int VehicleCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        //Raja: Removed virtual to avoid LazyLoading. Load this property eager when you need it
        public ICollection<BaseVehicle> BaseVehicles { get; set; }
    }
}
