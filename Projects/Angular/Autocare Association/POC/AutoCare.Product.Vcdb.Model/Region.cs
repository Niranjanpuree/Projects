using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class Region : EntityBase,IDomainEntity
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string RegionAbbr { get; set; }
        public string RegionAbbr_2 { get; set; }
        public int VehicleCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public virtual Region Parent { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }

    }
}
