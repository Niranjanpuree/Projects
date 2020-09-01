using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class SpringTypeConfiguration : EntityTypeConfiguration<SpringType>
    {
        public SpringTypeConfiguration()
            : this("dbo")
        {
        }

        public SpringTypeConfiguration(string schema)
        {
            ToTable(schema + ".SpringType");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("SpringTypeId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("SpringTypeName").IsRequired();
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.FrontSpringConfigCount);
            Ignore(x => x.RearSpringConfigCount);
            Ignore(x => x.VehicleToSpringConfigCount);
        }
    }
}
