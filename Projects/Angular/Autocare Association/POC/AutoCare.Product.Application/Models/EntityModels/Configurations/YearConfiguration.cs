using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class YearConfiguration : EntityTypeConfiguration<Year>
    {
        public YearConfiguration()
            : this("dbo")
        {
            
        }

        public YearConfiguration(string schema)
        {
            ToTable(schema + ".Year");
            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasColumnName("YearId")
                .IsRequired()
                .HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.BaseVehicleCount);
        }
    }
}
