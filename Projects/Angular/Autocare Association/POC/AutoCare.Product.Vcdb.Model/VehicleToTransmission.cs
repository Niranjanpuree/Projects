namespace AutoCare.Product.Vcdb.Model
{
    public class VehicleToTransmission : EntityBase, IDomainEntity
    {
        public int VehicleId { get; set; } // VehicleID
        public int TransmissionId { get; set; } // TransmissionID
        public string Source { get; set; } // Source (length: 10)
        public long? ChangeRequestId { get; set; }

        // Foreign keys
        public virtual Transmission Transmission { get; set; } // transmissionvehicleto_fk
        public virtual Vehicle Vehicle { get; set; } // vehicletotransmissionvehicle_fk
    }
}
