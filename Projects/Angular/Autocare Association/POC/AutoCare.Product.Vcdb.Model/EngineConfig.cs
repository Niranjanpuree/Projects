using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class EngineConfig : EntityBase, IDomainEntity
    {
        public int EngineDesignationId { get; set; } // EngineDesignationID
        public int EngineVinId { get; set; } // EngineVINID
        public int ValvesId { get; set; } // ValvesID
        public int EngineBaseId { get; set; } // EngineBaseID
        public int FuelDeliveryConfigId { get; set; } // FuelDeliveryConfigID
        public int AspirationId { get; set; } // AspirationID
        public int CylinderHeadTypeId { get; set; } // CylinderHeadTypeID
        public int FuelTypeId { get; set; } // FuelTypeID
        public int IgnitionSystemTypeId { get; set; } // IgnitionSystemTypeID
        public int EngineMfrId { get; set; } // EngineMfrID
        public int EngineVersionId { get; set; } // EngineVersionID
        public int PowerOutputId { get; set; } // PowerOutputID
        public long? ChangeRequestId { get; set; }
        public int VehicleToEngineConfigCount { get; set; }

        // Reverse navigation
        public ICollection<VehicleToEngineConfig> VehicleToEngineConfigs { get; set; } // VehicleToEngineConfig.engineconfigvehicleto_fk

        // Foreign keys
        public virtual Aspiration Aspiration { get; set; } // FK_Aspiration_EngineConfig
        public virtual CylinderHeadType CylinderHeadType { get; set; } // FK_CylinderHeadType_EngineConfig
        public virtual EngineBase EngineBase { get; set; } // FK_EngineBase_EngineConfig
        public virtual EngineDesignation EngineDesignation { get; set; } // FK_EngineDesignation_EngineConfig
        public virtual EngineVersion EngineVersion { get; set; } // FK_EngineVersion_EngineConfig
        public virtual EngineVin EngineVin { get; set; } // FK_EngineVIN_EngineConfig
        public virtual FuelDeliveryConfig FuelDeliveryConfig { get; set; } // FK_FuelDeliveryConfig_EngineConfig
        public virtual FuelType FuelType { get; set; } // FK_FuelType_EngineConfig
        public virtual IgnitionSystemType IgnitionSystemType { get; set; } // FK_IgnitionSystemType_EngineConfig
        public virtual Mfr Mfr { get; set; } // FK_Mfr_EngineConfig
        public virtual PowerOutput PowerOutput { get; set; }
        public virtual Valve Valves { get; set; } // FK_EngineConfig_Valves
    }
}
