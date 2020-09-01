using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class SpringConfig : EntityBase, IDomainEntity
    {
        public int FrontSpringTypeId { get; set; }
        public int RearSpringTypeId { get; set; }
        //public int SpringSystemId { get; set; }
       
        public int VehicleToSpringConfigCount { get; set; }
        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        public ICollection<VehicleToSpringConfig> VehicleToSpringConfigs { get; set; }
       
       // public virtual SpringSystem SpringSystem { get; set; }
        public virtual SpringType FrontSpringType { get; set; }
        public virtual SpringType RearSpringType { get; set; }
    }
}
