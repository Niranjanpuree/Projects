using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class TransmissionMfrCodeConfiguration : EntityTypeConfiguration<TransmissionMfrCode>
    {
        public TransmissionMfrCodeConfiguration()
            : this("dbo")
        {
        }

        public TransmissionMfrCodeConfiguration(string schema)
        {
            ToTable("TransmissionMfrCode", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"TransmissionMfrCodeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.TransmissionManufacturerCode).HasColumnName(@"TransmissionMfrCode").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(30);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.TransmissionCount);
        }
    }
}
