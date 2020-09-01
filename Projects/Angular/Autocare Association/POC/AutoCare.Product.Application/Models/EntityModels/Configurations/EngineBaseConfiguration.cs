using AutoCare.Product.Vcdb.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class EngineBaseConfiguration : EntityTypeConfiguration<EngineBase>
    {
        public EngineBaseConfiguration()
            : this("dbo")
        {
        }

        public EngineBaseConfiguration(string schema)
        {
            ToTable("EngineBase", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"EngineBaseID").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Liter).HasColumnName(@"Liter").IsRequired().HasColumnType("nvarchar").HasMaxLength(6);
            Property(x => x.Cc).HasColumnName(@"CC").IsRequired().HasColumnType("nvarchar").HasMaxLength(8);
            Property(x => x.Cid).HasColumnName(@"CID").HasColumnType("nvarchar").HasMaxLength(7);
            Property(x => x.Cylinders).HasColumnName(@"Cylinders").IsRequired().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(2);
            Property(x => x.BlockType).HasColumnName(@"BlockType").IsRequired().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(2);
            Property(x => x.EngBoreIn).HasColumnName(@"EngBoreIn").IsRequired().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.EngBoreMetric).HasColumnName(@"EngBoreMetric").IsRequired().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.EngStrokeIn).HasColumnName(@"EngStrokeIn").IsRequired().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.EngStrokeMetric).HasColumnName(@"EngStrokeMetric").IsRequired().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.EngineConfigCount);
        }
    }
}
