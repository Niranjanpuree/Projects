using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class FuelDeliveryType : EntityBase, IDomainEntity
    {
        public string FuelDeliveryTypeName { get; set; } // FuelDeliveryTypeName (length: 50).
        public long? ChangeRequestId { get; set; }
        public int FuelDeliveryConfigCount { get; set; }

        // Reverse navigation
        public ICollection<FuelDeliveryConfig> FuelDeliveryConfigs { get; set; } // FuelDeliveryConfig.FK_FuelDeliveryType_FuelDeliveryConfig
    }
}
