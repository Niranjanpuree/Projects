using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class TransmissionControlType : EntityBase, IDomainEntity
    {
        public string TransmissionControlTypeName { get; set; } // TransmissionControlTypeName (length: 30)
        public long? ChangeRequestId { get; set; }
        public int TransmissionBaseCount { get; set; }

        // Reverse navigation
        public ICollection<TransmissionBase> TransmissionBases { get; set; } // TransmissionBase.FK_TransmissionControlType_TransmissionBase
    }
}
