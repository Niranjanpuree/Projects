using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class PowerOutputConfiguration : EntityTypeConfiguration<PowerOutput>
    {
        public PowerOutputConfiguration()
            : this("dbo")
        {
        }

        public PowerOutputConfiguration(string schema)
        {
            ToTable("PowerOutput", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"PowerOutputID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.HorsePower).HasColumnName(@"HorsePower").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(10);
            Property(x => x.KilowattPower).HasColumnName(@"KilowattPower").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(10);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");
        }
    }
}
