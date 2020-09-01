using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class FuelType : EntityBase, IDomainEntity
    {
        public string FuelTypeName { get; set; } // FuelTypeName (length: 30)
        public long? ChangeRequestId { get; set; }
        public int EngineConfigCount { get; set; }

        public int VehicleToEngineConfigCount { get; set; }


        // Reverse navigation
        public ICollection<EngineConfig> EngineConfigs { get; set; } // EngineConfig.FK_FuelType_EngineConfig
    }
}
