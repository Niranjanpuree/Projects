using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class EngineVinConfiguration : EntityTypeConfiguration<EngineVin>
    {
        public EngineVinConfiguration()
            : this("dbo")
        {
        }

        public EngineVinConfiguration(string schema)
        {
            ToTable("EngineVIN", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"EngineVINID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.EngineVinName).HasColumnName(@"EngineVINName").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(5);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
            Ignore(x => x.VehicleToEngineConfigCount);
            Ignore(x => x.EngineConfigCount);
        }
    }
}
