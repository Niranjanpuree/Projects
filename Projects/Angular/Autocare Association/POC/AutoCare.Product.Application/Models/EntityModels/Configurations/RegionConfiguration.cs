using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class RegionConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Region>
    {
        public RegionConfiguration() : this("dbo")
        {

        }

        public RegionConfiguration(string schema)
        {
            ToTable(schema + ".Region");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("RegionId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("RegionName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ParentId).HasColumnName("ParentId").IsOptional().HasColumnType("int");
            Property(x => x.RegionAbbr).HasColumnName("RegionAbbr").IsRequired().HasColumnType("char").HasMaxLength(3);
            Property(x => x.RegionAbbr_2).HasColumnName("RegionAbbr_2").HasColumnType("char").HasMaxLength(3);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.VehicleCount);

            HasOptional(x => x.Parent).WithMany().HasForeignKey(p => p.ParentId);
        }
    }
}
