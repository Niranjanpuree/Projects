using System.ComponentModel.DataAnnotations.Schema;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.EntityModels.Configurations
{
    public class CommentsConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Comments>
    {
        public CommentsConfiguration() : this("dbo")
        {

        }

        public CommentsConfiguration(string schema)
        {
            ToTable("Comments");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("CommentId").IsRequired().HasColumnType("bigint").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ChangeRequestId).HasColumnName("ChangeRequestId").IsRequired().HasColumnType("bigint");
            Property(x => x.AddedBy).HasColumnName("AddedBy").IsRequired().HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.Comment).HasColumnName("Comment").IsRequired().HasColumnType("varchar(max)");
            Property(x => x.CreatedDatetime).HasColumnName("CreatedDatetime").IsRequired().HasColumnType("datetime");

            //Relationship
            HasRequired(t => t.ChangeRequestStore)
               .WithMany(t => t.Comments)
               .HasForeignKey(t => t.ChangeRequestId);
        }
    }
}
