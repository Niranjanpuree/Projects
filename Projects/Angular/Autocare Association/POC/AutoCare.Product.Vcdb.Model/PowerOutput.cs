using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class PowerOutput : EntityBase, IDomainEntity
    {
        public string HorsePower { get; set; } // HorsePower (length: 10)
        public string KilowattPower { get; set; } // KilowattPower (length: 10)
        public long? ChangeRequestId { get; set; }

        // Reverse navigation
        public ICollection<EngineConfig> EngineConfigs { get; set; }
    }
}
