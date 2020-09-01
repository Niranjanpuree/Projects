using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class BaseVehicle : EntityBase, IDomainEntity
    {
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int YearId { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public virtual Make Make { get; set; }
        public virtual Model Model { get; set; }
        public virtual Year Year { get; set; }
        public int VehicleCount { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
