using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class FuelDeliveryConfig : EntityBase, IDomainEntity
    {
        public int FuelDeliveryTypeId { get; set; } // FuelDeliveryTypeID
        public int FuelDeliverySubTypeId { get; set; } // FuelDeliverySubTypeID
        public int FuelSystemControlTypeId { get; set; } // FuelSystemControlTypeID
        public int FuelSystemDesignId { get; set; } // FuelSystemDesignID
        public long? ChangeRequestId { get; set; }
        public int EngineConfigCount { get; set; }

        // Reverse navigation
        public virtual ICollection<EngineConfig> EngineConfigs { get; set; } // EngineConfig.FK_FuelDeliveryConfig_EngineConfig

        // Foreign keys
        public virtual FuelDeliverySubType FuelDeliverySubType { get; set; } // FK_FuelDeliverySubType_FuelDeliveryConfig
        public virtual FuelDeliveryType FuelDeliveryType { get; set; } // FK_FuelDeliveryType_FuelDeliveryConfig
        public virtual FuelSystemControlType FuelSystemControlType { get; set; } // FK_FuelSystemControlType_FuelDeliveryConfig
        public virtual FuelSystemDesign FuelSystemDesign { get; set; } // FK_FuelSystemDesign_FuelDeliveryConfig
    }
}
