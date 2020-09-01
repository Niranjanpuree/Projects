using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class ChangeRequestTypeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ChangeRequestType>
    {
        public ChangeRequestTypeConfiguration()
           : this("dbo")
        {
        }

        public ChangeRequestTypeConfiguration(string schema)
        {
            ToTable(schema + ".ChangeRequestType");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasColumnType("varchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").IsRequired().HasColumnType("varchar").HasMaxLength(250);
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.CreatedDateTime).HasColumnName("CreatedDateTime").IsRequired().HasColumnType("datetime");
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired().HasColumnType("bit");
        }
    }
}
