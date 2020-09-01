using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class EngineDesignation : EntityBase, IDomainEntity
    {
        public string EngineDesignationName { get; set; } // EngineDesignationName (length: 30)
        public long? ChangeRequestId { get; set; }
        public int EngineConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }

        // Reverse navigation
        public ICollection<EngineConfig> EngineConfigs { get; set; } // EngineConfig.FK_EngineDesignation_EngineConfig
    }
}
