using System;
using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class Vehicle : EntityBase, IDomainEntity
    {
        public int BaseVehicleId { get; set; }
        public int SubModelId { get; set; }
        public string SourceName { get; set; }
        public int SourceId { get; set; }
        public int RegionId { get; set; }
        public int PublicationStageId { get; set; }
        public string PublicationStageSource { get; set; }
        public DateTime PublicationStageDate { get; set; }
        public string PublicationEnvironment { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
        public int VehicleToBedConfigCount { get; set; }
        public int VehicleToBodyStyleConfigCount { get; set; }
        public int VehicleToWheelBaseCount { get; set; }
        public int VehicleToMfrBodyCodeCount { get; set; }
        public int VehicleToDriveTypeCount { get; set; }
        public int VehicleToSteeringConfigCount { get; set; }
        public int VehicleToSpringConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }
        public int VehicleToTransmissionCount { get; set; }

        [ChangeRequestProperty]
        public long? ChangeRequestId { get; set; }
        // Foreign keys
        public virtual BaseVehicle BaseVehicle { get; set; }
        public virtual SubModel SubModel { get; set; }
        public virtual Source Source { get; set; }
        public virtual Region Region { get; set; }
        public virtual PublicationStage PublicationStage { get; set; }
        public ICollection<VehicleToBrakeConfig> VehicleToBrakeConfigs { get; set; }
        public ICollection<VehicleToBedConfig> VehicleToBedConfigs { get; set; }
        public ICollection<VehicleToBodyStyleConfig> VehicleToBodyStyleConfigs { get; set; }
        public ICollection<VehicleToDriveType> VehicleToDriveTypes { get; set; }
        public ICollection<VehicleToMfrBodyCode> VehicleToMfrBodyCodes { get; set; }
        public ICollection<VehicleToWheelBase> VehicleToWheelBases { get; set; }
        public ICollection<VehicleToSteeringConfig> VehicleToSteeringConfigs { get; set; }
        public ICollection<VehicleToSpringConfig> VehicleToSpringConfigs { get; set; }
        public virtual ICollection<VehicleToEngineConfig> VehicleToEngineConfigs { get; set; }
        public virtual ICollection<VehicleToTransmission> VehicleToTransmissions { get; set; }
    }
}
