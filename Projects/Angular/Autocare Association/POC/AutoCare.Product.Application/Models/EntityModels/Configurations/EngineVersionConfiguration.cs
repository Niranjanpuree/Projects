using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class EngineVersionConfiguration : EntityTypeConfiguration<EngineVersion>
    {
        public EngineVersionConfiguration()
            : this("dbo")
        {
        }

        public EngineVersionConfiguration(string schema)
        {
            ToTable("EngineVersion", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"EngineVersionID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.EngineVersionName).HasColumnName(@"EngineVersion").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
            Ignore(x => x.VehicleToEngineConfigCount);
            Ignore(x => x.EngineConfigCount);
        }
    }
}
