using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class Mfr : EntityBase, IDomainEntity
    {
        public string MfrName { get; set; } // MfrName (length: 30)
        public long? ChangeRequestId { get; set; }
        public int EngineConfigCount { get; set; }
        public int TransmissionCount { get; set; }

        // Reverse navigation
        public ICollection<EngineConfig> EngineConfigs { get; set; } // EngineConfig.FK_Mfr_EngineConfig
        public ICollection<Transmission> Transmissions { get; set; } // Transmission.FK_Mfr_Transmission
    }
}
