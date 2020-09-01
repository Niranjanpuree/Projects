using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class CylinderHeadType : EntityBase, IDomainEntity
    {
        public string CylinderHeadTypeName { get; set; } // CylinderHeadTypeName (length: 30)
        public long? ChangeRequestId { get; set; }
        public int EngineConfigCount { get; set; }

        // Reverse navigation
        public ICollection<EngineConfig> EngineConfigs { get; set; } // EngineConfig.FK_CylinderHeadType_EngineConfig
    }
}
