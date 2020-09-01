using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class VehicleType: EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        //Raja modified to avoid 2 way navigation. Relationship is done in configuration
        //>Pushkar: ICollection<Model> Models re-added for calculating its count
        public ICollection<Model> Models { get; set; }
        public int VehicleTypeGroupId { get; set; }
        public int ModelCount { get; set; }
        public int BaseVehicleCount { get; set; }
        public int VehicleCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public virtual VehicleTypeGroup VehicleTypeGroup { get; set; }
    }
}
