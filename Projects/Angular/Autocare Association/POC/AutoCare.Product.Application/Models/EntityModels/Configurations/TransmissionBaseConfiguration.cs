using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class TransmissionBaseConfiguration : EntityTypeConfiguration<TransmissionBase>
    {
        public TransmissionBaseConfiguration()
            : this("dbo")
        {
        }

        public TransmissionBaseConfiguration(string schema)
        {
            ToTable("TransmissionBase", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"TransmissionBaseID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.TransmissionTypeId).HasColumnName(@"TransmissionTypeID").IsRequired().HasColumnType("int");
            Property(x => x.TransmissionNumSpeedsId).HasColumnName(@"TransmissionNumSpeedsID").IsRequired().HasColumnType("int");
            Property(x => x.TransmissionControlTypeId).HasColumnName(@"TransmissionControlTypeID").IsRequired().HasColumnType("int");
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            // Foreign keys
            HasRequired(a => a.TransmissionControlType).WithMany(b => b.TransmissionBases).HasForeignKey(c => c.TransmissionControlTypeId).WillCascadeOnDelete(false); // FK_TransmissionControlType_TransmissionBase
            HasRequired(a => a.TransmissionNumSpeeds).WithMany(b => b.TransmissionBases).HasForeignKey(c => c.TransmissionNumSpeedsId).WillCascadeOnDelete(false); // FK_TransmissionNumSpeeds_TransmissionBase
            HasRequired(a => a.TransmissionType).WithMany(b => b.TransmissionBases).HasForeignKey(c => c.TransmissionTypeId).WillCascadeOnDelete(false); // FK_TransmissionType_TransmissionBase

            Ignore(x => x.TransmissionCount);
        }
    }
}
