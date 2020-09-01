using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class TransmissionNumSpeeds : EntityBase, IDomainEntity
    {
        public string TransmissionNumberOfSpeeds { get; set; } // TransmissionNumSpeeds (length: 3)
        public long? ChangeRequestId { get; set; }
        public int TransmissionBaseCount { get; set; }

        // Reverse navigation
        public ICollection<TransmissionBase> TransmissionBases { get; set; } // TransmissionBase.FK_TransmissionNumSpeeds_TransmissionBase
    }
}
