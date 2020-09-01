namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRequestItemStagingAndComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangeRequestItemStaging",
                c => new
                    {
                        ChangeRequestItemId = c.Long(nullable: false, identity: true),
                        ChangeRequestId = c.Long(nullable: false),
                        Entity = c.String(nullable: false, maxLength: 50, unicode: false),
                        EntityId = c.String(nullable: false, maxLength: 100, unicode: false),
                        Payload = c.String(nullable: false, unicode: false),
                        ChangeType = c.Short(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ChangeRequestItemId)
                .ForeignKey("dbo.ChangeRequestStaging", t => t.ChangeRequestId, cascadeDelete: true)
                .Index(t => t.ChangeRequestId);
            
            CreateTable(
                "dbo.CommentsStaging",
                c => new
                    {
                        CommentId = c.Long(nullable: false, identity: true),
                        ChangeRequestId = c.Long(nullable: false),
                        AddedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        Comment = c.String(nullable: false, unicode: false),
                        CreatedDatetime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.ChangeRequestStaging", t => t.ChangeRequestId, cascadeDelete: true)
                .Index(t => t.ChangeRequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChangeRequestItemStaging", "ChangeRequestId", "dbo.ChangeRequestStaging");
            DropForeignKey("dbo.CommentsStaging", "ChangeRequestId", "dbo.ChangeRequestStaging");
            DropIndex("dbo.CommentsStaging", new[] { "ChangeRequestId" });
            DropIndex("dbo.ChangeRequestItemStaging", new[] { "ChangeRequestId" });
            DropTable("dbo.CommentsStaging");
            DropTable("dbo.ChangeRequestItemStaging");
        }
    }
}
