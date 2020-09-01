using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class FuelSystemDesign : EntityBase, IDomainEntity
    {
        public string FuelSystemDesignName { get; set; } // FuelSystemDesignName (length: 50)
        public long? ChangeRequestId { get; set; }
        public int FuelDeliveryConfigCount { get; set; }

        // Reverse navigation
        public ICollection<FuelDeliveryConfig> FuelDeliveryConfigs { get; set; } // FuelDeliveryConfig.FK_FuelSystemDesign_FuelDeliveryConfig
    }
}
