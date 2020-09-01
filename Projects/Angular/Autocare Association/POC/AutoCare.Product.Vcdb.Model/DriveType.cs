using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class DriveType : EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int VehicleToDriveTypeCount { get; set; }
        public long? ChangeRequestId { get; set; }
        public ICollection<VehicleToDriveType> VehicleToDriveTypes { get; set; }
    }
}
