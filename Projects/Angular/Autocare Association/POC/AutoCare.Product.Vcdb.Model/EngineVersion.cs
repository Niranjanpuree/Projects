using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class EngineVersion : EntityBase, IDomainEntity
    {
        public string EngineVersionName { get; set; } // EngineVersion (length: 20)
        public long? ChangeRequestId { get; set; }
        public int EngineConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }

        // Reverse navigation
        public ICollection<EngineConfig> EngineConfigs { get; set; } // EngineConfig.FK_EngineVersion_EngineConfig
    }
}
