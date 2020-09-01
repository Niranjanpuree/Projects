namespace AutoCare.Product.Application.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRequestStore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        AttachmentId = c.Long(nullable: false),
                        ChangeRequestId = c.Long(nullable: false),
                        OriginalFileName = c.String(nullable: false, maxLength: 512),
                        FileExtension = c.String(nullable: false, maxLength: 10),
                        FilePath = c.String(nullable: false, maxLength: 1024),
                        AttachedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedDatetime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentId)
                .ForeignKey("dbo.ChangeRequestStore", t => t.ChangeRequestId, cascadeDelete: true)
                .Index(t => t.ChangeRequestId);
            
            CreateTable(
                "dbo.ChangeRequestItem",
                c => new
                    {
                        ChangeRequestItemId = c.Long(nullable: false),
                        ChangeRequestId = c.Long(nullable: false),
                        Entity = c.String(nullable: false, maxLength: 50, unicode: false),
                        EntityId = c.String(nullable: false, maxLength: 100, unicode: false),
                        Payload = c.String(nullable: false, unicode: false),
                        ChangeType = c.Short(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ChangeRequestItemId)
                .ForeignKey("dbo.ChangeRequestStore", t => t.ChangeRequestId, cascadeDelete: true)
                .Index(t => t.ChangeRequestId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Long(nullable: false),
                        ChangeRequestId = c.Long(nullable: false),
                        AddedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        Comment = c.String(nullable: false, unicode: false),
                        CreatedDatetime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.ChangeRequestStore", t => t.ChangeRequestId, cascadeDelete: true)
                .Index(t => t.ChangeRequestId);
            
            CreateTable(
                "dbo.TaskControllers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Entity = c.String(),
                        RequestedBy = c.String(),
                        ReceivedDate = c.DateTime(nullable: false),
                        CompletededDate = c.DateTime(nullable: false),
                        Status = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AttachmentsStaging",
                c => new
                    {
                        AttachmentId = c.Long(nullable: false, identity: true),
                        ChangeRequestId = c.Long(nullable: false),
                        OriginalFileName = c.String(nullable: false, maxLength: 512),
                        FileExtension = c.String(nullable: false, maxLength: 10),
                        FilePath = c.String(nullable: false, maxLength: 1024),
                        AttachedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedDatetime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentId)
                .ForeignKey("dbo.ChangeRequestStaging", t => t.ChangeRequestId, cascadeDelete: true)
                .Index(t => t.ChangeRequestId);
            
            AddColumn("dbo.ChangeRequestStaging", "Status", c => c.Short(nullable: false));
            CreateIndex("dbo.ChangeRequestStore", "TaskControllerId");
            CreateIndex("dbo.ChangeRequestStaging", "TaskControllerId");
            AddForeignKey("dbo.ChangeRequestStaging", "TaskControllerId", "dbo.TaskControllers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChangeRequestStore", "TaskControllerId", "dbo.TaskControllers", "Id", cascadeDelete: true);
            DropColumn("dbo.ChangeRequestStaging", "Payload");
            DropColumn("dbo.ChangeRequestStore", "Payload");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChangeRequestStore", "Payload", c => c.String(nullable: false, unicode: false));
            AddColumn("dbo.ChangeRequestStaging", "Payload", c => c.String(nullable: false, unicode: false));
            DropForeignKey("dbo.Attachments", "ChangeRequestId", "dbo.ChangeRequestStore");
            DropForeignKey("dbo.ChangeRequestStore", "TaskControllerId", "dbo.TaskControllers");
            DropForeignKey("dbo.ChangeRequestStaging", "TaskControllerId", "dbo.TaskControllers");
            DropForeignKey("dbo.AttachmentsStaging", "ChangeRequestId", "dbo.ChangeRequestStaging");
            DropForeignKey("dbo.Comments", "ChangeRequestId", "dbo.ChangeRequestStore");
            DropForeignKey("dbo.ChangeRequestItem", "ChangeRequestId", "dbo.ChangeRequestStore");
            DropIndex("dbo.AttachmentsStaging", new[] { "ChangeRequestId" });
            DropIndex("dbo.ChangeRequestStaging", new[] { "TaskControllerId" });
            DropIndex("dbo.Comments", new[] { "ChangeRequestId" });
            DropIndex("dbo.ChangeRequestItem", new[] { "ChangeRequestId" });
            DropIndex("dbo.ChangeRequestStore", new[] { "TaskControllerId" });
            DropIndex("dbo.Attachments", new[] { "ChangeRequestId" });
            DropColumn("dbo.ChangeRequestStaging", "Status");
            DropTable("dbo.AttachmentsStaging");
            DropTable("dbo.TaskControllers");
            DropTable("dbo.Comments");
            DropTable("dbo.ChangeRequestItem");
            DropTable("dbo.Attachments");
        }
    }
}
