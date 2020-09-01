namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class removeLikes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Likes", "ChangeRequestId", "dbo.ChangeRequestStore");
            DropForeignKey("dbo.LikesStaging", "ChangeRequestId", "dbo.ChangeRequestStaging");
            DropIndex("dbo.Likes", new[] { "ChangeRequestId" });
            DropIndex("dbo.LikesStaging", new[] { "ChangeRequestId" });
            DropTable("dbo.Likes");
            DropTable("dbo.LikesStaging");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LikesStaging",
                c => new
                    {
                        LikesStagingId = c.Long(nullable: false),
                        ChangeRequestId = c.Long(nullable: false),
                        LikeStatus = c.Byte(nullable: false),
                        LikedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedDatetime = c.DateTime(nullable: false),
                        UpdatedDatetime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LikesStagingId);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        LikesId = c.Long(nullable: false),
                        ChangeRequestId = c.Long(nullable: false),
                        LikeStatus = c.Byte(nullable: false),
                        LikedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedDatetime = c.DateTime(nullable: false),
                        UpdatedDatetime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LikesId);
            
            CreateIndex("dbo.LikesStaging", "ChangeRequestId");
            CreateIndex("dbo.Likes", "ChangeRequestId");
            AddForeignKey("dbo.LikesStaging", "ChangeRequestId", "dbo.ChangeRequestStaging", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Likes", "ChangeRequestId", "dbo.ChangeRequestStore", "Id", cascadeDelete: true);
        }
    }
}
