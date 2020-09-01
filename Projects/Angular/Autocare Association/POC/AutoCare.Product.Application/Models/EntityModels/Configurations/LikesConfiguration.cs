using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class LikesConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Likes>
    {
        public LikesConfiguration(): this("dbo")
        {
                
        }

        public LikesConfiguration(string schema)
        {
            ToTable("Likes");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("LikesId").IsRequired().HasColumnType("bigint").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").IsRequired().HasColumnType("bigint");
            Property(x => x.LikeStatus).HasColumnName("LikeStatus").IsRequired().HasColumnType("tinyint");
            Property(x => x.LikedBy).HasColumnName("LikedBy").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.CreatedDatetime).HasColumnName("CreatedDatetime").IsRequired().HasColumnType("datetime");
            Property(x => x.UpdatedDatetime).HasColumnName("UpdatedDatetime").HasColumnType("datetime");

            //Relationship
            HasRequired(t => t.ChangeRequestStore)
               .WithMany(t => t.Likes)
               .HasForeignKey(t => t.ChangeRequestId);

        }
    }
}
