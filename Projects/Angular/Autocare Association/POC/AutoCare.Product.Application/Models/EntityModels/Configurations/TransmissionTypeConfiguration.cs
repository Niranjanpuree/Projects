using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class TransmissionTypeConfiguration : EntityTypeConfiguration<TransmissionType>
    {
        public TransmissionTypeConfiguration()
            : this("dbo")
        {
        }

        public TransmissionTypeConfiguration(string schema)
        {
            ToTable("TransmissionType", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"TransmissionTypeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.TransmissionTypeName).HasColumnName(@"TransmissionTypeName").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(30);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.TransmissionBaseCount);
        }
    }
}
