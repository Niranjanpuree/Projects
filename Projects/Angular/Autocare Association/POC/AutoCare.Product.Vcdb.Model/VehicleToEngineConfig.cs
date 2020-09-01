namespace AutoCare.Product.Vcdb.Model
{
    public class VehicleToEngineConfig : EntityBase, IDomainEntity
    {
        public int VehicleId { get; set; } // VehicleID
        public int EngineConfigId { get; set; } // EngineConfigID
        public string Source { get; set; } // Source (length: 10)
        public long? ChangeRequestId { get; set; }

        // Foreign keys
        public virtual EngineConfig EngineConfig { get; set; } // engineconfigvehicleto_fk
        public virtual Vehicle Vehicle { get; set; } // vehicletoengineconfigvehicle_fk
    }
}
