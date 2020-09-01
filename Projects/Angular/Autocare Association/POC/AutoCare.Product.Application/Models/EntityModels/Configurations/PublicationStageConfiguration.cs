using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class PublicationStageConfiguration: EntityTypeConfiguration<PublicationStage>
    {
        public PublicationStageConfiguration()
           : this("dbo")
        {
        }

        public PublicationStageConfiguration(string schema)
        {
            ToTable(schema + ".PublicationStage");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("PublicationStageId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("PublicationStageName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
        }
    }
}
