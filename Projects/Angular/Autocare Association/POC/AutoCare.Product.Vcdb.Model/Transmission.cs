using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class Transmission : EntityBase, IDomainEntity
    {
        public int TransmissionBaseId { get; set; } // TransmissionBaseID
        public int TransmissionMfrCodeId { get; set; } // TransmissionMfrCodeID
        public int TransmissionElecControlledId { get; set; } // TransmissionElecControlledID
        public int TransmissionMfrId { get; set; } // TransmissionMfrID
        public long? ChangeRequestId { get; set; }
        public int VehicleToTransmissionCount { get; set; }

        // Reverse navigation
        public ICollection<VehicleToTransmission> VehicleToTransmissions { get; set; } // VehicleToTransmission.transmissionvehicleto_fk

        // Foreign keys
        public virtual ElecControlled ElecControlled { get; set; } // FK_Transmission_ElecControlled
        public virtual Mfr Mfr { get; set; } // FK_Mfr_Transmission
        public virtual TransmissionBase TransmissionBase { get; set; } // FK_TransmissionBase_Transmission
        public virtual TransmissionMfrCode TransmissionMfrCode { get; set; } // FK_TransmissionMfrCode_Transmission
    }
}
