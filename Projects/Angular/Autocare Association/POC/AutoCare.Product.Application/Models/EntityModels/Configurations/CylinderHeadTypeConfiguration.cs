using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class CylinderHeadTypeConfiguration : EntityTypeConfiguration<CylinderHeadType>
    {
        public CylinderHeadTypeConfiguration()
            : this("dbo")
        {
        }

        public CylinderHeadTypeConfiguration(string schema)
        {
            ToTable("CylinderHeadType", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"CylinderHeadTypeID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.CylinderHeadTypeName).HasColumnName(@"CylinderHeadTypeName").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(30);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.EngineConfigCount);
        }
    }
}
