using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class SubModelConfiguration : EntityTypeConfiguration<SubModel>
    {
        public SubModelConfiguration()
            : this("dbo")
        {

        }

        public SubModelConfiguration(string schema)
        {
            ToTable(schema + ".SubModel");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("SubModelId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("SubModelName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.VehicleCount);
        }
    }
}
