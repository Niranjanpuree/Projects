using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class TransmissionControlTypeConfiguration : EntityTypeConfiguration<TransmissionControlType>
    {
        public TransmissionControlTypeConfiguration()
            : this("dbo")
        {
        }

        public TransmissionControlTypeConfiguration(string schema)
        {
            ToTable("TransmissionControlType", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"TransmissionControlTypeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.TransmissionControlTypeName).HasColumnName(@"TransmissionControlTypeName").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(30);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.TransmissionBaseCount);
        }
    }
}
