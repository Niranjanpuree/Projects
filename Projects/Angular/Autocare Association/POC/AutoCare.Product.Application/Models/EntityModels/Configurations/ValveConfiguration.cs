using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class ValveConfiguration : EntityTypeConfiguration<Valve>
    {
        public ValveConfiguration()
            : this("dbo")
        {
        }

        public ValveConfiguration(string schema)
        {
            ToTable("Valves", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"ValvesID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ValvesPerEngine).HasColumnName(@"ValvesPerEngine").IsRequired().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(3);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.EngineConfigCount);
        }
    }
}
