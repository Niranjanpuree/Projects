using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class RoleConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
           : this("dbo")
        {
        }

        public RoleConfiguration(string schema)
        {
            ToTable(schema + ".Role");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("smallint").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Description).HasColumnName("Description").IsRequired().HasColumnType("nvarchar").HasMaxLength(225);
            Property(x => x.CreatedDateTime).HasColumnName("CreatedDateTime").IsRequired().HasColumnType("datetime");
            Property(x => x.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime");
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired().HasColumnType("bit");
        }

    }
}
