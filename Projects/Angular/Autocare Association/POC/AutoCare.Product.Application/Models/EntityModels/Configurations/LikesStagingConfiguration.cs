using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class LikesStagingConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<LikesStaging>
    {
        public LikesStagingConfiguration(): this("dbo")
        {
                
        }

        public LikesStagingConfiguration(string schema)
        {
            ToTable("LikesStaging");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("LikesStagingId").IsRequired().HasColumnType("bigint").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").IsRequired().HasColumnType("bigint");
            Property(x => x.LikeStatus).HasColumnName("LikeStatus").IsRequired().HasColumnType("tinyint");
            Property(x => x.LikedBy).HasColumnName("LikedBy").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.CreatedDatetime).HasColumnName("CreatedDatetime").IsRequired().HasColumnType("datetime");
            Property(x => x.UpdatedDatetime).HasColumnName("UpdatedDatetime").HasColumnType("datetime");

            //Relationship
            HasRequired(t => t.ChangeRequestStaging)
               .WithMany(t => t.LikesStagings)
               .HasForeignKey(t => t.ChangeRequestId);

        }
    }
}
