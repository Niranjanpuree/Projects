using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class SpringConfigConfiguration : EntityTypeConfiguration<SpringConfig>
    {
        public SpringConfigConfiguration()
            : this("dbo")
        {
        }

        public SpringConfigConfiguration(string schema)
        {
            ToTable(schema + ".SpringConfig");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("SpringConfigId").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.FrontSpringTypeId).HasColumnName("FrontSpringTypeId").IsRequired().HasColumnType("int");
            Property(x => x.RearSpringTypeId).HasColumnName("RearSpringTypeId").IsRequired().HasColumnType("int");
            //Property(x => x.SpringSystemId).HasColumnName("SpringSystemId").IsRequired().HasColumnType("int");
           
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").HasColumnType("bigint");

            Ignore(x => x.VehicleToSpringConfigCount);

            // Foreign keys
           
            //HasRequired(a => a.SpringSystem).WithMany(b => b.SpringConfigs).HasForeignKey(c => c.SpringSystemId);
            HasRequired(a => a.FrontSpringType).WithMany(b => b.SpringConfigs_FrontSpringTypeId).HasForeignKey(c => c.FrontSpringTypeId).WillCascadeOnDelete(false);
            HasRequired(a => a.RearSpringType).WithMany(b => b.SpringConfigs_RearSpringTypeId).HasForeignKey(c => c.RearSpringTypeId).WillCascadeOnDelete(false);
        }
    }
}
