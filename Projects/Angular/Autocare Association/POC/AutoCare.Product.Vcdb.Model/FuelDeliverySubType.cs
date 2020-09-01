using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class FuelDeliverySubType : EntityBase, IDomainEntity
    {
        public string FuelDeliverySubTypeName { get; set; } // FuelDeliverySubTypeName (length: 50)
        public long? ChangeRequestId { get; set; }
        public int FuelDeliveryConfigCount { get; set; }

        // Reverse navigation
        public ICollection<FuelDeliveryConfig> FuelDeliveryConfigs { get; set; } // FuelDeliveryConfig.FK_FuelDeliverySubType_FuelDeliveryConfig
    }
}
