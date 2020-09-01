using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class EngineVin : EntityBase, IDomainEntity
    {
        public string EngineVinName { get; set; } // EngineVINName (length: 5)
        public long? ChangeRequestId { get; set; }
        public int EngineConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }

        // Reverse navigation
        public ICollection<EngineConfig> EngineConfigs { get; set; } // EngineConfig.FK_EngineVIN_EngineConfig
    }
}
