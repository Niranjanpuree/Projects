using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class UserConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<User>
    {
        public UserConfiguration()
           : this("dbo")
        {
        }

        public UserConfiguration(string schema)
        {
            ToTable(schema + ".User");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(225);
            Property(x => x.CreatedDateTime).HasColumnName("CreatedDateTime").IsRequired().HasColumnType("datetime");
            Property(x => x.UpdatedDateTime).HasColumnName("UpdatedDateTime").HasColumnType("datetime");
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired().HasColumnType("bit");
        }
    }
}
