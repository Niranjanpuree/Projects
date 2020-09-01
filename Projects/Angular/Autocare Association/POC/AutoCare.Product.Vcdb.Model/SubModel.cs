using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class SubModel : EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int VehicleCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
