using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class Aspiration : EntityBase, IDomainEntity
    {
        public string AspirationName { get; set; } // AspirationName (length: 30)
        public long? ChangeRequestId { get; set; }
        public int EngineConfigCount { get; set; }

        // Reverse navigation
        public ICollection<EngineConfig> EngineConfigs { get; set; } // EngineConfig.FK_Aspiration_EngineConfig
    }
}
