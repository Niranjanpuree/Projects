using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class SourceConfiguration : EntityTypeConfiguration<Source>
    {
        public SourceConfiguration()
            :this("dbo")
        {

        }

        public SourceConfiguration(string schema)
        {
            ToTable(schema + ".Source");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("SourceId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("SourceName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
        }
    }
}
