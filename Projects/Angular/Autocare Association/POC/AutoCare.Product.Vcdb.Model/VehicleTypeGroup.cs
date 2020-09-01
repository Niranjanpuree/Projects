using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class VehicleTypeGroup: EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int VehicleTypeCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        //Raja modified to avoid 2 way navigation. Relationship is done in configuration
        //>Pushkar: ICollection<VehicleType> VehicleTypes re-added for calculating its count
        public ICollection<VehicleType> VehicleTypes { get; set; }
    }
}
