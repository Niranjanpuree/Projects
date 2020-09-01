using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class MfrBodyCode : EntityBase, IDomainEntity
    {
        public string Name { get; set; }
        public int VehicleToMfrBodyCodeCount { get; set; }
        public long? ChangeRequestId { get; set; }
        public ICollection<VehicleToMfrBodyCode> VehicleToMfrBodyCodes { get; set; }
    }
}
