using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class Year : EntityBase,IDomainEntity
    {
        public int BaseVehicleCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<BaseVehicle> BaseVehicles { get; set; }
    }
}
