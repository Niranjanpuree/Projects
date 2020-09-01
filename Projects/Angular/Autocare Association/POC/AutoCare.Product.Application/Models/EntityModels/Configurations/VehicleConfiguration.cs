using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleConfiguration : EntityTypeConfiguration<Vehicle>
    {
        public VehicleConfiguration()
            :this("dbo")
        {
            
        }

        public VehicleConfiguration(string schema)
        {

            ToTable(schema + ".Vehicle");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"VehicleId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.BaseVehicleId).HasColumnName(@"BaseVehicleId").IsRequired().HasColumnType("int");
            Property(x => x.SubModelId).HasColumnName(@"SubModelId").IsRequired().HasColumnType("int");
            Property(x => x.RegionId).HasColumnName(@"RegionId").IsRequired().HasColumnType("int");
            Property(x => x.SourceName).HasColumnName(@"Source").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.SourceId).HasColumnName(@"SourceId").IsRequired().HasColumnType("int");
            Property(x => x.PublicationStageId).HasColumnName(@"PublicationStageId").IsRequired().HasColumnType("int");
            Property(x => x.PublicationStageSource).HasColumnName(@"PublicationStageSource").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.PublicationStageDate).HasColumnName(@"PublicationStageDate").IsRequired().HasColumnType("datetime");
            Property(x => x.PublicationEnvironment).HasColumnName(@"PublicationEnvironment").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.VehicleToBrakeConfigCount);
            Ignore(x => x.VehicleToBedConfigCount);
            Ignore(x => x.VehicleToBodyStyleConfigCount);
            Ignore(x => x.VehicleToDriveTypeCount);
            Ignore(x => x.VehicleToMfrBodyCodeCount);
            Ignore(x => x.VehicleToWheelBaseCount);
            Ignore(x => x.VehicleToSteeringConfigCount);
            Ignore(x => x.VehicleToSpringConfigCount);
            Ignore(x => x.VehicleToEngineConfigCount);
            Ignore(x => x.VehicleToTransmissionCount);

            // Foreign keys
            HasRequired(a => a.BaseVehicle).WithMany(b => b.Vehicles).HasForeignKey(c => c.BaseVehicleId); 
            HasRequired(a => a.PublicationStage).WithMany(b => b.Vehicles).HasForeignKey(c => c.PublicationStageId);
            HasRequired(a => a.Region).WithMany(b => b.Vehicles).HasForeignKey(c => c.RegionId); 
            HasRequired(a => a.SubModel).WithMany(b => b.Vehicles).HasForeignKey(c => c.SubModelId);
            HasRequired(a => a.Source).WithMany(b => b.Vehicles).HasForeignKey(c => c.SourceId);
        }
    }
}
