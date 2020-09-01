using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class VehicleToTransmissionConfiguration : EntityTypeConfiguration<VehicleToTransmission>
    {
        public VehicleToTransmissionConfiguration()
            : this("dbo")
        {
        }

        public VehicleToTransmissionConfiguration(string schema)
        {
            ToTable("VehicleToTransmission", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"VehicleToTransmissionID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.VehicleId).HasColumnName(@"VehicleID").IsRequired().HasColumnType("int");
            Property(x => x.TransmissionId).HasColumnName(@"TransmissionID").IsRequired().HasColumnType("int");
            Property(x => x.Source).HasColumnName("Source").HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            // Foreign keys
            HasRequired(a => a.Transmission).WithMany(b => b.VehicleToTransmissions).HasForeignKey(c => c.TransmissionId).WillCascadeOnDelete(false); // transmissionvehicleto_fk
            HasRequired(a => a.Vehicle).WithMany(b => b.VehicleToTransmissions).HasForeignKey(c => c.VehicleId).WillCascadeOnDelete(false); // vehicletotransmissionvehicle_fk
        }
    }
}
