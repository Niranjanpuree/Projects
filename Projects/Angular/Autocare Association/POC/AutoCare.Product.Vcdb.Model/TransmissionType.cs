using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class TransmissionType : EntityBase, IDomainEntity
    {
        public string TransmissionTypeName { get; set; } // TransmissionTypeName (length: 30)
        public long? ChangeRequestId { get; set; }
        public int TransmissionBaseCount { get; set; }

        // Reverse navigation
        public ICollection<TransmissionBase> TransmissionBases { get; set; } // TransmissionBase.FK_TransmissionType_TransmissionBase
    }
}
