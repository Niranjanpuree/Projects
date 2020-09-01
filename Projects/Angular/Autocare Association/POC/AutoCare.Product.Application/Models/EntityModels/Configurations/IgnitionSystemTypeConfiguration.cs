using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class IgnitionSystemTypeConfiguration : EntityTypeConfiguration<IgnitionSystemType>
    {
        public IgnitionSystemTypeConfiguration()
            : this("dbo")
        {
        }

        public IgnitionSystemTypeConfiguration(string schema)
        {
            ToTable("IgnitionSystemType", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"IgnitionSystemTypeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.IgnitionSystemTypeName).HasColumnName(@"IgnitionSystemTypeName").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(30);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.EngineConfigCount);
        }
    }
}
