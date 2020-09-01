using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class Make : EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int BaseVehicleCount { get; set; }
        public int VehicleCount { get; set; }

        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<BaseVehicle> BaseVehicles { get; set; }
    }
}
