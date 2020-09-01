using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class Valve : EntityBase, IDomainEntity
    {
        public string ValvesPerEngine { get; set; } // ValvesPerEngine (length: 3)
        public long? ChangeRequestId { get; set; }
        public int EngineConfigCount { get; set; }

        // Reverse navigation
        public ICollection<EngineConfig> EngineConfigs { get; set; }
    }
}
