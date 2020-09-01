namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class readdLikes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        LikesId = c.Long(nullable: false),
                        ChangeRequestId = c.Long(nullable: false),
                        LikeStatus = c.Byte(nullable: false),
                        LikedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedDatetime = c.DateTime(nullable: false),
                        UpdatedDatetime = c.DateTime(),
                    })
                .PrimaryKey(t => t.LikesId)
                .ForeignKey("dbo.ChangeRequestStore", t => t.ChangeRequestId, cascadeDelete: true)
                .Index(t => t.ChangeRequestId);
            
            CreateTable(
                "dbo.LikesStaging",
                c => new
                    {
                        LikesStagingId = c.Long(nullable: false, identity: true),
                        ChangeRequestId = c.Long(nullable: false),
                        LikeStatus = c.Byte(nullable: false),
                        LikedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedDatetime = c.DateTime(nullable: false),
                        UpdatedDatetime = c.DateTime(),
                    })
                .PrimaryKey(t => t.LikesStagingId)
                .ForeignKey("dbo.ChangeRequestStaging", t => t.ChangeRequestId, cascadeDelete: true)
                .Index(t => t.ChangeRequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LikesStaging", "ChangeRequestId", "dbo.ChangeRequestStaging");
            DropForeignKey("dbo.Likes", "ChangeRequestId", "dbo.ChangeRequestStore");
            DropIndex("dbo.LikesStaging", new[] { "ChangeRequestId" });
            DropIndex("dbo.Likes", new[] { "ChangeRequestId" });
            DropTable("dbo.LikesStaging");
            DropTable("dbo.Likes");
        }
    }
}
