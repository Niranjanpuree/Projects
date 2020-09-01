using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class TransmissionBase : EntityBase, IDomainEntity
    {
        public int TransmissionTypeId { get; set; } // TransmissionTypeID
        public int TransmissionNumSpeedsId { get; set; } // TransmissionNumSpeedsID
        public int TransmissionControlTypeId { get; set; } // TransmissionControlTypeID
        public long? ChangeRequestId { get; set; }
        public int TransmissionCount { get; set; }

        // Reverse navigation
        public ICollection<Transmission> Transmissions { get; set; } // Transmission.FK_TransmissionBase_Transmission

        // Foreign keys
        public virtual TransmissionControlType TransmissionControlType { get; set; } // FK_TransmissionControlType_TransmissionBase
        public virtual TransmissionNumSpeeds TransmissionNumSpeeds { get; set; } // FK_TransmissionNumSpeeds_TransmissionBase
        public virtual TransmissionType TransmissionType { get; set; } // FK_TransmissionType_TransmissionBase
    }
}
