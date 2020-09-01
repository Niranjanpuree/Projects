using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class MfrConfiguration : EntityTypeConfiguration<Mfr>
    {
        public MfrConfiguration()
            : this("dbo")
        {
        }

        public MfrConfiguration(string schema)
        {
            ToTable("Mfr", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"MfrID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.MfrName).HasColumnName(@"MfrName").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(30);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.EngineConfigCount);
            Ignore(x => x.TransmissionCount);
        }
    }
}
