using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class FuelSystemControlType : EntityBase, IDomainEntity
    {
        public string FuelSystemControlTypeName { get; set; } // FuelSystemControlTypeName (length: 50)
        public long? ChangeRequestId { get; set; }
        public int FuelDeliveryConfigCount { get; set; }

        // Reverse navigation
        public ICollection<FuelDeliveryConfig> FuelDeliveryConfigs { get; set; } // FuelDeliveryConfig.FK_FuelSystemControlType_FuelDeliveryConfig
    }
}
