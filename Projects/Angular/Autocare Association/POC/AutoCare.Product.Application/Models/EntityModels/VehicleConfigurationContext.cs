using System.Data.Entity;
using AutoCare.Product.Application.Models.EntityModels.Configurations;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels
{
    public class VehicleConfigurationContext : DbContext
    {
        public VehicleConfigurationContext() : base("VcdbConnection")
        {
            //this.Configuration.LazyLoadingEnabled = false; 
            //this.Configuration.ProxyCreationEnabled = false; 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BaseVehicleConfiguration());
            modelBuilder.Configurations.Add(new MakeConfiguration());
            modelBuilder.Configurations.Add(new YearConfiguration());
            modelBuilder.Configurations.Add(new ModelConfiguration());
            modelBuilder.Configurations.Add(new VehicleTypeConfiguration());
            modelBuilder.Configurations.Add(new VehicleTypeGroupConfiguration());
            modelBuilder.Configurations.Add(new SubModelConfiguration());
            modelBuilder.Configurations.Add(new RegionConfiguration());
            modelBuilder.Configurations.Add(new SourceConfiguration());
            modelBuilder.Configurations.Add(new PublicationStageConfiguration());
            modelBuilder.Configurations.Add(new VehicleConfiguration());
            modelBuilder.Configurations.Add(new ChangeRequestStagingConfiguration());
            modelBuilder.Configurations.Add(new ChangeRequestStoreConfiguration());
            modelBuilder.Configurations.Add(new ChangeRequestTypeConfiguration());

            modelBuilder.Configurations.Add(new VehicleToBrakeConfigConfiguration());
            modelBuilder.Configurations.Add(new BrakeConfigConfiguration());
            modelBuilder.Configurations.Add(new BrakeTypeConfiguration());
            modelBuilder.Configurations.Add(new BrakeSystemConfiguration());
            modelBuilder.Configurations.Add(new BrakeABSConfiguration());
            modelBuilder.Configurations.Add(new ChangeRequestItemStagingConfiguration());
            modelBuilder.Configurations.Add(new CommentsStagingConfiguration());
            modelBuilder.Configurations.Add(new AttachmentsStagingConfiguration());

            modelBuilder.Configurations.Add(new ChangeRequestItemConfiguration());
            modelBuilder.Configurations.Add(new CommentsConfiguration());
            modelBuilder.Configurations.Add(new AttachmentsConfiguration());

            modelBuilder.Configurations.Add(new VehicleToSpringConfigConfiguration());
            modelBuilder.Configurations.Add(new SpringConfigConfiguration());
            modelBuilder.Configurations.Add(new SpringTypeConfiguration());

            modelBuilder.Configurations.Add(new TaskControllerConfiguration());

            modelBuilder.Configurations.Add(new LikesConfiguration());
            modelBuilder.Configurations.Add(new LikesStagingConfiguration());

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new ChangeRequestAssignmentStagingConfiguration());
            modelBuilder.Configurations.Add(new ChangeRequestAssignmentConfiguration());

            modelBuilder.Configurations.Add(new ChangeRequestRolesAssignmentConfiguration());
            modelBuilder.Configurations.Add(new UserRoleAssignmentConfiguration());

            modelBuilder.Configurations.Add(new BedTypeConfiguration());
            modelBuilder.Configurations.Add(new BedLengthConfiguration());
            modelBuilder.Configurations.Add(new BedConfigConfiguration());
            modelBuilder.Configurations.Add(new VehicleToBedConfigConfiguration());

            modelBuilder.Configurations.Add(new BodyTypeConfiguration());
            modelBuilder.Configurations.Add(new BodyNumDoorsConfiguration());
            modelBuilder.Configurations.Add(new BodyStyleConfigConfiguration());
            modelBuilder.Configurations.Add(new VehicleToBodyStyleConfigConfiguration());

            modelBuilder.Configurations.Add(new DriveTypeConfiguration());
            modelBuilder.Configurations.Add(new WheelBaseConfiguration());
            modelBuilder.Configurations.Add(new MfrBodyCodeConfiguration());

            modelBuilder.Configurations.Add(new VehicleToWheelBaseConfiguration());
            modelBuilder.Configurations.Add(new VehicleToMfrBodyCodeConfiguration());
            modelBuilder.Configurations.Add(new VehicleToDriveTypeConfiguration());

            modelBuilder.Configurations.Add(new VehicleToSteeringConfigConfiguration());
            modelBuilder.Configurations.Add(new SteeringConfigConfiguration());
            modelBuilder.Configurations.Add(new SteeringTypeConfiguration());
            modelBuilder.Configurations.Add(new SteeringSystemConfiguration());

            modelBuilder.Configurations.Add(new AspirationConfiguration());
            modelBuilder.Configurations.Add(new VehicleToEngineConfigConfiguration());
            modelBuilder.Configurations.Add(new EngineConfigConfiguration());
            modelBuilder.Configurations.Add(new EngineBaseConfiguration());
            modelBuilder.Configurations.Add(new CylinderHeadTypeConfiguration());
            modelBuilder.Configurations.Add(new EngineDesignationConfiguration());
            modelBuilder.Configurations.Add(new EngineVersionConfiguration());
            modelBuilder.Configurations.Add(new EngineVinConfiguration());
            modelBuilder.Configurations.Add(new FuelDeliveryConfigConfiguration());
            modelBuilder.Configurations.Add(new FuelTypeConfiguration());
            modelBuilder.Configurations.Add(new IgnitionSystemTypeConfiguration());
            modelBuilder.Configurations.Add(new MfrConfiguration());
            modelBuilder.Configurations.Add(new ValveConfiguration());
            modelBuilder.Configurations.Add(new FuelDeliverySubTypeConfiguration());
            modelBuilder.Configurations.Add(new FuelDeliveryTypeConfiguration());
            modelBuilder.Configurations.Add(new FuelSystemControlTypeConfiguration());
            modelBuilder.Configurations.Add(new FuelSystemDesignConfiguration());
            modelBuilder.Configurations.Add(new PowerOutputConfiguration());
            modelBuilder.Configurations.Add(new VehicleToTransmissionConfiguration());
            modelBuilder.Configurations.Add(new TransmissionConfiguration());
            modelBuilder.Configurations.Add(new ElecControlledConfiguration());
            modelBuilder.Configurations.Add(new TransmissionBaseConfiguration());
            modelBuilder.Configurations.Add(new TransmissionMfrCodeConfiguration());
            modelBuilder.Configurations.Add(new TransmissionControlTypeConfiguration());
            modelBuilder.Configurations.Add(new TransmissionNumSpeedsConfiguration());
            modelBuilder.Configurations.Add(new TransmissionTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BaseVehicle> BaseVehicles { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<VehicleTypeGroup> VehicleTypeGroups { get; set; }
        public DbSet<SubModel> SubModels { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<PublicationStage> PublicationStages { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<ChangeRequestStaging> ChangeRequestStagings { get; set; }
        public DbSet<ChangeRequestStore> ChangeRequestStores { get; set; }
        public DbSet<ChangeRequestType> ChangeRequestTypes { get; set; }

        public DbSet<VehicleToBrakeConfig> VehicleToBrakeConfigs { get; set; }
        public DbSet<BrakeConfig> BrakeConfigs { get; set; }
        public DbSet<BrakeType> BrakeTypes { get; set; }
        public DbSet<BrakeSystem> BrakeSystems { get; set; }
        public DbSet<BrakeABS> BrakeABSes { get; set; }
        public DbSet<ChangeRequestItemStaging> ChangeRequestItemStagings { get; set; }
        public DbSet<CommentsStaging> CommentsStagings { get; set; }
        public DbSet<AttachmentsStaging> AttachmentsStagings { get; set; }

        public DbSet<ChangeRequestItem> ChangeRequestItems { get; set; }
        public DbSet<Comments> Comments { get; set;}
        public DbSet<Attachments> Attachments { get; set; }

        public DbSet<TaskController> TaskControllers { get; set; }

        public DbSet<Likes> Likes { get; set; }
        public DbSet<LikesStaging> LikesStagings { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<ChangeRequestAssignmentStaging> ChangeRequestAssignmentStagings { get; set; }
        public DbSet<ChangeRequestAssignment> ChangeRequestAssignments { get; set; }
        public DbSet<UserRoleAssignment> UserRoleAssignments { get; set; }
        public DbSet<ChangeRequestRolesAssignment> ChangeRequestRolesAssignments { get; set; }
        public DbSet<BedLength> BedLengths { get; set; }
        public DbSet<BedType> BedTypes { get; set; }
        public DbSet<BedConfig> BedConfigs { get; set; }
        public DbSet<VehicleToBedConfig> VehicleToBedConfigs { get; set; }

        public DbSet<BodyNumDoors> BodyNumDoors { get; set; }
        public DbSet<BodyType> BodyTypes { get; set; }
        public DbSet<BodyStyleConfig> BodyStyleConfigs { get; set; }
        public DbSet<VehicleToBodyStyleConfig> VehicleToBodyStyleConfigs { get; set; }
        public DbSet<WheelBase> WheelBases { get; set; }
        public DbSet<VehicleToWheelBase> VehicleToWheelBases { get; set; }
        public DbSet<MfrBodyCode> MfrBodyCodes { get; set; }
        public DbSet<VehicleToMfrBodyCode> VehicleToMfrBodyCodes { get; set; }
        public DbSet<DriveType> DriveTypes { get; set; }
        public DbSet<VehicleToDriveType> VehicleToDriveTypes { get; set; }

        public DbSet<VehicleToSteeringConfig> VehicleToSteeringConfigs { get; set; }
        public DbSet<SteeringConfig> SteeringConfigs { get; set; }
        public DbSet<SteeringType> SteeringTypes { get; set; }
        public DbSet<SteeringSystem> SteeringSystems { get; set; }

        public DbSet<VehicleToSpringConfig> VehicleToSpringConfigs { get; set; }
        public DbSet<SpringConfig> SpringConfigs { get; set; }
        public DbSet<SpringType> SpringTypes { get; set; }

        public DbSet<Aspiration> Aspirations { get; set; }
        public DbSet<VehicleToEngineConfig> VehicleToEngineConfigs { get; set; }
        public DbSet<EngineConfig> EngineConfigs { get; set; }
        public DbSet<EngineBase> EngineBases { get; set; }
        public DbSet<CylinderHeadType> CylinderHeadTypes { get; set; }
        public DbSet<EngineDesignation> EngineDesignations { get; set; }
        public DbSet<EngineVersion> EngineVersions { get; set; }
        public DbSet<EngineVin> EngineVins { get; set; }
        public DbSet<FuelDeliveryConfig> FuelDeliveryConfigs { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<IgnitionSystemType> IgnitionSystemTypes { get; set; }
        public DbSet<Mfr> Mfrs { get; set; }
        public DbSet<Valve> Valves { get; set; }
        public DbSet<FuelDeliverySubType> FuelDeliverySubTypes { get; set; }
        public DbSet<FuelDeliveryType> FuelDeliveryTypes { get; set; }
        public DbSet<FuelSystemControlType> FuelSystemControlTypes { get; set; }
        public DbSet<FuelSystemDesign> FuelSystemDesigns { get; set; }
        public DbSet<PowerOutput> PowerOutputs { get; set; }
        public DbSet<VehicleToTransmission> VehicleToTransmissions { get; set; }
        public DbSet<Transmission> Transmissions { get; set; }
        public DbSet<ElecControlled> ElecControlleds { get; set; }
        public DbSet<TransmissionBase> TransmissionBases { get; set; }
        public DbSet<TransmissionMfrCode> TransmissionMfrCodes { get; set; }
        public DbSet<TransmissionControlType> TransmissionControlTypes { get; set; }
        public DbSet<TransmissionNumSpeeds> TransmissionNumSpeeds { get; set; }
        public DbSet<TransmissionType> TransmissionTypes { get; set; }

    }
}