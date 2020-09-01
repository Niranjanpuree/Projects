using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class EngineBase : EntityBase, IDomainEntity
    {
        public string Liter { get; set; } // Liter (length: 6)
        public string Cc { get; set; } // CC (length: 8)
        public string Cid { get; set; } // CID (length: 7)
        public string Cylinders { get; set; } // Cylinders (length: 2)
        public string BlockType { get; set; } // BlockType (length: 2)
        public string EngBoreIn { get; set; } // EngBoreIn (length: 10)
        public string EngBoreMetric { get; set; } // EngBoreMetric (length: 10)
        public string EngStrokeIn { get; set; } // EngStrokeIn (length: 10)
        public string EngStrokeMetric { get; set; } // EngStrokeMetric (length: 10)
        public long? ChangeRequestId { get; set; }
        public int EngineConfigCount { get; set; }

        // Reverse navigation
        public ICollection<EngineConfig> EngineConfigs { get; set; } // EngineConfig.FK_EngineBase_EngineConfig
    }
}
