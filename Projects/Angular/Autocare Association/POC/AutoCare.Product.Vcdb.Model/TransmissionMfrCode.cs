using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class TransmissionMfrCode : EntityBase, IDomainEntity
    {
        public string TransmissionManufacturerCode { get; set; } // TransmissionMfrCode (length: 30)
        public long? ChangeRequestId { get; set; }
        public int TransmissionCount { get; set; }

        // Reverse navigation
        public ICollection<Transmission> Transmissions { get; set; } // Transmission.FK_TransmissionMfrCode_Transmission
    }
}
