using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class ElecControlled : EntityBase, IDomainEntity
    {
        public string ElectronicallyControlled { get; set; } // ElecControlled (length: 3)
        public long? ChangeRequestId { get; set; }
        public int TransmissionCount { get; set; }

        // Reverse navigation
        public ICollection<Transmission> Transmissions { get; set; } // Transmission.FK_Transmission_ElecControlled
    }
}
