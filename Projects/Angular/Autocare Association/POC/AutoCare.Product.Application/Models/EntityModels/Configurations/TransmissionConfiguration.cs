using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class TransmissionConfiguration : EntityTypeConfiguration<Transmission>
    {
        public TransmissionConfiguration()
            : this("dbo")
        {
        }

        public TransmissionConfiguration(string schema)
        {
            ToTable("Transmission", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"TransmissionID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.TransmissionBaseId).HasColumnName(@"TransmissionBaseID").IsRequired().HasColumnType("int");
            Property(x => x.TransmissionMfrCodeId).HasColumnName(@"TransmissionMfrCodeID").IsRequired().HasColumnType("int");
            Property(x => x.TransmissionElecControlledId).HasColumnName(@"TransmissionElecControlledID").IsRequired().HasColumnType("int");
            Property(x => x.TransmissionMfrId).HasColumnName(@"TransmissionMfrID").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            // Foreign keys
            HasRequired(a => a.ElecControlled).WithMany(b => b.Transmissions).HasForeignKey(c => c.TransmissionElecControlledId).WillCascadeOnDelete(false); // FK_Transmission_ElecControlled
            HasRequired(a => a.Mfr).WithMany(b => b.Transmissions).HasForeignKey(c => c.TransmissionMfrId).WillCascadeOnDelete(false); // FK_Mfr_Transmission
            HasRequired(a => a.TransmissionBase).WithMany(b => b.Transmissions).HasForeignKey(c => c.TransmissionBaseId).WillCascadeOnDelete(false); // FK_TransmissionBase_Transmission
            HasRequired(a => a.TransmissionMfrCode).WithMany(b => b.Transmissions).HasForeignKey(c => c.TransmissionMfrCodeId).WillCascadeOnDelete(false); // FK_TransmissionMfrCode_Transmission

            Ignore(x => x.VehicleToTransmissionCount);
        }
    }
}
