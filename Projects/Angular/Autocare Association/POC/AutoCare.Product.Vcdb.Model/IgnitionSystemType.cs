using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class IgnitionSystemType : EntityBase, IDomainEntity
    {
        public string IgnitionSystemTypeName { get; set; } // IgnitionSystemTypeName (length: 30)
        public long? ChangeRequestId { get; set; }
        public int EngineConfigCount { get; set; }

        // Reverse navigation
        public ICollection<EngineConfig> EngineConfigs { get; set; } // EngineConfig.FK_IgnitionSystemType_EngineConfig
    }
}
