using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class AspirationConfiguration : EntityTypeConfiguration<Aspiration>
    {
        public AspirationConfiguration()
            : this("dbo")
        {
        }

        public AspirationConfiguration(string schema)
        {
            ToTable("Aspiration", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"AspirationID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.AspirationName).HasColumnName(@"AspirationName").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(30);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.EngineConfigCount);
        }
    }
}
